using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;


namespace DataManagement
{
    // Data types that can be created through reading off a csv styled text document
    namespace DataType
    {
        // this is a self made enum type class for the item types 
        public class ItemType
        {
            public static ItemType Weapon = new ItemType(0, "WEAPON"); // capitalize all letters for the item type that way toupper() can be called later to prevent string comparrisons from breaking this.
            public static ItemType ChestArmor = new ItemType(1, "CHESTARMOR");
            public static ItemType LegArmor = new ItemType(2, "LEGARMOR");
            public static ItemType HeadArmor = new ItemType(3, "HEADARMOR");
            public static ItemType OffHand = new ItemType(4, "OFFHAND");
            public static ItemType Consumable = new ItemType(5, "CONSUMABLE");

            public string typeOfItem { get; private set; }
            public int value { get; private set; }

            public static IEnumerable<ItemType> List()
            {
                return new[] { Weapon, ChestArmor, LegArmor, HeadArmor, OffHand, Consumable };
            }

            private ItemType(int val, string type)
            {
                typeOfItem = type;
                value = val;
            }

            public static ItemType typeFromString(string typeOfItem)
            {
                return List().Single(type => String.Equals(type.typeOfItem, typeOfItem, StringComparison.OrdinalIgnoreCase));
            }

            public static ItemType typeFromValue(int val)
            {
                return List().Single(type => type.value == val);
            }
        }

        public struct ItemData
        {
            public string itemName;
            public string itemValue;
            public ItemType itemType;
            public string itemFilepath;
            public static int dataFieldCount = 4; // these are the number of fields that will be needed to create this object excluding this field

            public ItemData(string name, string value, ItemType type, string filepath)
            {
                itemName = name;
                itemValue = value;
                itemType = type;
                itemFilepath = filepath;
            }
        }

        public struct TileGenerationData
        {
            public string tileEditorTag;
            public string tileEditorLayer;
            public string tileFilepath;
            public static int dataFieldCount = 3; // these are the number of fields that will be needed to create this object excluding this field

            public TileGenerationData(string editorTag, string editorLayer, string filepath)
            {
                tileEditorTag = editorTag;
                tileEditorLayer = editorLayer;
                tileFilepath = filepath;
            }
        }
    }

    namespace DataReader
    {
        public class TDReader
        {
            private string textDocumentPath;

            public TDReader(string filePath)
            {
                textDocumentPath = filePath;
            }

            public List<DataType.ItemData> getItemData()
            {
                string itemDataString = readCSVFile();

                List<DataType.ItemData> itemData = createItemData(ref itemDataString, DataType.ItemData.dataFieldCount);

                return itemData;
            }

            public List<DataType.TileGenerationData> getTileData()
            {
                string tileDataString = readCSVFile();

                List<DataType.TileGenerationData> tileData = createTileGenData(ref tileDataString, DataType.TileGenerationData.dataFieldCount);

                return tileData;
            }

            // @param: numberOfDataFields - the number of values needed to make the object in question
            private List<DataType.TileGenerationData> createTileGenData(ref string incomingDataString, int numberOfDataFields)
            {
                string[] seperatedTileData = splitCSVString(ref incomingDataString, numberOfDataFields);

                if(seperatedTileData.Length < 1)
                {
                    List<DataType.TileGenerationData> emptyList = new List<DataType.TileGenerationData>();
                    return emptyList;
                }

                Queue<string> tileDataQueue = createDataQueue(ref seperatedTileData);

                int tileDataLength = tileDataQueue.Count / numberOfDataFields;

                List<DataType.TileGenerationData> tileData = new List<DataType.TileGenerationData>(tileDataLength);

                string editorTag = "";
                string editorLayer = "";
                string tileFilePath = "";

                for(int i = 1; i <= tileDataLength; ++i)
                {
                    editorTag = tileDataQueue.Dequeue();
                    editorLayer = tileDataQueue.Dequeue(); // TODO: if something weird happens its because you never get rid of the previous stored values in these...maybe consider it
                    tileFilePath = tileDataQueue.Dequeue();
                    tileData.Add(new DataType.TileGenerationData(editorTag, editorLayer, tileFilePath));
                }

                return tileData;

            }

            // @param: numberOfDataFields - the number of values needed to make the object in question
            private List<DataType.ItemData> createItemData(ref string incomingDataString, int numberOfDataFields)
            {
                string[] seperatedItemData = splitCSVString(ref incomingDataString, numberOfDataFields);

                if(seperatedItemData.Length < 1)
                {
                    List<DataType.ItemData> emptyList = new List<DataType.ItemData>();
                    return emptyList; // TODO: eventually have an error handling system, but for now just return and empty string list
                }

                Queue<string> itemDataQueue = createDataQueue(ref seperatedItemData);

                int itemDataLength = itemDataQueue.Count / numberOfDataFields;

                List<DataType.ItemData> itemData = new List<DataType.ItemData>(itemDataLength);

                string itemName = "";
                string itemValue = "";
                DataType.ItemType itemType;
                string itemFilePath = "";

                for (int i = 1; i <= itemDataLength; ++i) // i use the itemDataLength to keep track of the number of times need to go through the loop
                {
                    itemName = itemDataQueue.Dequeue();
                    itemValue = itemDataQueue.Dequeue(); // TODO: if something weird happens its because you never get rid of the previous stored values in these...maybe consider it
                    itemType = DataType.ItemType.typeFromString(itemDataQueue.Dequeue().ToUpper());
                    itemFilePath = itemDataQueue.Dequeue();
                    itemData.Add(new DataType.ItemData(itemName, itemValue, itemType, itemFilePath));
                }


                return itemData;
            }

            // utility functions for checking if data types are valid and to manipulate the string on the document
            private string readCSVFile()
            {
                string documentString = "";

                if (textDocumentPath != "")
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(textDocumentPath))
                        {
                            documentString = sr.ReadToEnd();

                            if (documentString == "")
                                documentString = "The file read had no data on it.";
                        }
                    }
                    catch (Exception e)
                    {
                        string errorMessage = "This file could not be read. Exeption error: " + e.Message;
                        System.Diagnostics.Debug.WriteLine(errorMessage);
                    }
                }

                return documentString;
            }

            private string[] splitCSVString(ref string stringToSplit, int numberOfDataFields)
            {
                char[] delimiters = { ',', '\n', '\r' };
                string[] splitDocumentData = { };

                if(isValidCSVData(ref stringToSplit, numberOfDataFields))
                {
                    splitDocumentData = stringToSplit.Split(delimiters);
                    return splitDocumentData;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("There is an error with the data on the csv document.");
                    return splitDocumentData; // this is blank intentionally its to let you know something went wrong when checking the csv file's contents
                }
                
            }

            private Queue<string> createDataQueue(ref string[] dataArray)
            {
                IEnumerable<string> filteredItemData = dataArray.Where(item => item != "");

                Queue<string> dataQueue = new Queue<string>(dataArray.Length);

                foreach (string itemCell in filteredItemData)
                {
                    dataQueue.Enqueue(itemCell);
                }

                if(dataQueue.Count < 1)
                {
                    Queue<string> emptyQueue = new Queue<string>();
                    return emptyQueue; // TODO: eventually have an error handling system, but for now just return and empty string queue
                }

                return dataQueue;
            }

            // Checks the data entires after they have been retrieved from file.
            private bool isValidCSVData(ref string dataString, int objectProperties) //todo: debug this
            {
                int numberOfMatches = 0;
                int currentRow = 0;
                bool isValid = true;
                bool endsOfArray = false;
                string errorString = "The following rows in the csv document have errors: ";

                for (int i = 0; i < dataString.Length; i++)
                {
                    endsOfArray = ((i + 1) >= dataString.Length || (i - 1) < 0);

                    if (endsOfArray != true)
                    {
                        if (dataString[i] == ',' && dataString[i - 1] != ' ')
                        {
                            numberOfMatches++;
                        }

                        if ((dataString[i - 1] != ' ' || dataString[i - 1] != ',') && dataString[i + 1] == '\n')
                        {
                            numberOfMatches++;
                            currentRow++;

                            if (numberOfMatches == objectProperties)
                            {
                                numberOfMatches = 0;
                            }
                            else
                            {
                                errorString += currentRow.ToString() + ", ";
                                isValid = false; // i dont see a reason to manually set this to true anywhere because if this triggers than the document has an error
                                numberOfMatches = 0;
                            }
                        }
                    }
                }

                if (isValid == false)
                {
                    string formattedErrorString = errorString.TrimEnd(' ', ',');
                    System.Diagnostics.Debug.WriteLine(formattedErrorString);
                }

                return isValid;
            }
        }
    }
}

