using System;
using UnityEngine;

namespace Gametator 
{
    public class FileManager : SingletonComponent<FileManager>
    {
        [SerializeField] private LEBase leBase;
        public LEBase LEBase { get { return leBase; }}
        [Space]

        public int LastLoadedLevel = 0;
        public int CurrMoveCount = 0;
        public int CurrRowCount = 0;
        public int CurrColumnCount = 0;
        public int[,] CurrGridInfo;

        public bool LoadDataFromLocal(int currentLevel)
        {
            Debug.Log("CurrentLevel: " + currentLevel);

            //Read data from text file
            TextAsset mapText = Resources.Load("LevelData/" + currentLevel) as TextAsset;
            if (mapText == null)
            {
                Debug.LogError("MapText: Null");
                return false;
            }

            LastLoadedLevel = currentLevel;
            ResetTheVariables();
            ProcessGameDataFromString(mapText.text);
            return true;
        }

        void ProcessGameDataFromString(string mapText)
        {
            string[] lines = mapText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            int mapLine = 0;

            foreach (string line in lines)
            {
                if (line.StartsWith("MOVE COUNT "))
                {
                    string moveCountString = line.Replace("MOVE COUNT", string.Empty).Trim();
                    CurrMoveCount = int.Parse(moveCountString);
                }
                else if (line.StartsWith("ROWS "))
                {
                    string rowsString = line.Replace("ROWS", string.Empty).Trim();
                    CurrRowCount = int.Parse(rowsString);
                }
                else if (line.StartsWith("COLUMNS "))
                {
                    string columnsString = line.Replace("COLUMNS", string.Empty).Trim();
                    CurrColumnCount = int.Parse(columnsString);

                    CurrGridInfo = new int[CurrColumnCount, CurrRowCount];
                }
                else if(line.StartsWith("MAPITEM"))
                { 
                    string str = line.Replace("MAPITEM", string.Empty);
                    string[] st = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    for(int x=0; x < st.Length; x++)
                    {
                        CurrGridInfo[x, mapLine] = int.Parse(st[x].ToString());
                    }

                    mapLine++;
                }
            }
        }

        private void ResetTheVariables()
        {
            CurrMoveCount = 0;
            CurrRowCount = 0;
            CurrColumnCount = 0;

            CurrGridInfo = null;
        }

        public int GetLastCompletedLevel()
        {
            return LastLoadedLevel;
        }

        public LEItemModel GetModel(int dataIndex)
        {
            return LEBase.ItemModels[dataIndex];
        }
    }
}