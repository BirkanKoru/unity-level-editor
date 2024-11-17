using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
public class LEController : EditorWindow
{
    private string LevelEditorBasePrefabPath = "Assets/Scripts/LevelEditor/Prefabs/LEBase.prefab";
    private LEBase LevelEditorBase;

    private static int selectedTab;
    private string[] toolbarStrings = new string[] { "Levels", "Items" };
    private Vector2 scrollViewVector;

    private int levelNumber = 0;

    private int moveCount = 1;
    private int rowCount = 1;
    private int columnCount = 1;
    private int maxRowColumnCount = 10;

    private LEItemModel selectedItemModel;
    private string selectedItemName = "";
    private LEItemType selectedItemType = LEItemType.Color;
    private int brush;

    // SAVE VARIABLES
    private string fileName = "1.txt";
    public static int[] levelSquares = new int[400];


    [MenuItem("Puzzle/LevelEditor")]
    public static void ShowWindow()
    {
        GetWindow<LEController>("LevelEditor");
    }

    private void OnFocus() 
    {
        Initialize();

        if(LoadDataFromLocal(1))
            levelNumber = 1;
        else
            AddLevel(false, 1);
    }
    
    private void Initialize()
    {
        LevelEditorBase = AssetDatabase.LoadAssetAtPath(LevelEditorBasePrefabPath, typeof(LEBase)) as LEBase;

        if(LevelEditorBase == null)
            Debug.LogError("LevelEditorBase Prefab not found!!! Please check the path: " + LevelEditorBasePrefabPath);

        if(LevelEditorBase.ItemModels == null)
            LevelEditorBase.ItemModels = new List<LEItemModel>();

        moveCount   = 1;
        rowCount    = 1;
        columnCount = 1;

        for (int i = 0; i < levelSquares.Length; i++)
            levelSquares[i] = 0;
    }

    private void OnGUI() 
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        selectedTab = GUILayout.Toolbar(selectedTab, toolbarStrings, new GUILayoutOption[] { GUILayout.Width(350) });
        GUILayout.EndHorizontal();

        scrollViewVector = GUI.BeginScrollView(new Rect(25, 45, position.width - 30, position.height), scrollViewVector, new Rect(0, 0, 400, 500 + (rowCount * 50)));
        GUILayout.Space(-30);

        if(selectedTab == 0)
        {
            GUILevelSelector();
            GUILayout.Space(10);

            GUIMoveCount();
            GUILayout.Space(10);

            GUIItems();
            GUILayout.Space(10);

            GUIMap();
            GUILayout.Space(10);

            GUIGameField();
            GUILayout.Space(10);

        } else if(selectedTab == 1) {

            GUIItemTab();
            GUILayout.Space(10);
        }

        GUI.EndScrollView();
    }

    #region LevelSelector
    private void GUILevelSelector()
    {
        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LEVEL:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(50) });
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        if (GUILayout.Button("<<", new GUILayoutOption[] { GUILayout.Width(50) }))
            PreviousLevel();

        string changeLvl = GUILayout.TextField(" " + levelNumber, new GUILayoutOption[] { GUILayout.Width(50) });

        if (int.TryParse(changeLvl, out int inTextLevel))
        {
            if(inTextLevel != levelNumber)
            {
                if(LoadDataFromLocal(inTextLevel))
                    levelNumber = inTextLevel;
            }
        }

        if (GUILayout.Button(">>", new GUILayoutOption[] { GUILayout.Width(50) }))
            NextLevel();
        
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("New level", new GUILayoutOption[] { GUILayout.Width(100) }))
            AddLevel(true, GetLastLevel() + 1);
        
        GUI.backgroundColor = Color.white;

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(60);

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void AddLevel(bool saveLastLevel, int newLevelNumber)
    {
        if(saveLastLevel) SaveLevel();

        levelNumber = newLevelNumber;
        Initialize();
        SaveLevel();
    }

    private void NextLevel()
    {
        levelNumber++;

        if(!LoadDataFromLocal(levelNumber))
            levelNumber--;
    }

    private void PreviousLevel()
    {
        levelNumber--;
        if(levelNumber < 1) levelNumber = 1;

        if (!LoadDataFromLocal(levelNumber))
        {
            levelNumber++;
        }
    }
    #endregion

    #region  MoveCountEditor
    private void GUIMoveCount()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(60);

        GUILayout.Label("MOVE COUNT:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(100) });

        int moveCountBackup = moveCount;
        moveCount = EditorGUILayout.IntField(moveCount, new GUILayoutOption[] { GUILayout.Width(50) });
        if (moveCount <= 0) moveCount = 1;

        if(moveCountBackup != moveCount) SaveLevel();
        GUILayout.EndHorizontal();
    }
    #endregion

    #region ItemEditor
    private void GUIItemTab()
    {
        ShowItems();
        GUILayout.Space(20);

        if(selectedItemModel != null)
            ShowSelectedItemModelInfo();
    }

    private void GUIItems()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(60);
        GUILayout.BeginVertical();

        GUILayout.Label("ITEMS:", EditorStyles.label);

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        ShowItems();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void ShowItems()
    {
        GUILayout.BeginHorizontal();

        if(LevelEditorBase.ItemModels.Count > 0)
        {
            for(int i=0; i < LevelEditorBase.ItemModels.Count; i++)
            {
                LEItemModel itemModel = LevelEditorBase.ItemModels[i];

                if(itemModel.itemIcon != null)
                {
                    if (GUILayout.Button(itemModel.itemIcon.texture, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
                    {
                        SelectItemModel(itemModel);
                        brush = i;
                    }

                } else 
                {
                    if (GUILayout.Button("", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
                    {
                        SelectItemModel(itemModel);
                        brush = i;
                    }
                }
            }

            GUILayout.BeginVertical();
            if (GUILayout.Button(LevelEditorBase.itemAddIcon.texture, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(25) }))
            {
                selectedTab = 1;
                SelectItemModel(LevelEditorBase.AddNewItemModel());
                SaveItem();
            }
            if (GUILayout.Button(LevelEditorBase.itemRemoveIcon.texture, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(25) }))
            {
                LevelEditorBase.RemoveModel(LevelEditorBase.GetLastItemModel());
                selectedItemModel = LevelEditorBase.GetLastItemModel();

                SaveItem();
            }
            GUILayout.EndVertical();

        } else 
        {
            if (GUILayout.Button(LevelEditorBase.itemAddIcon.texture, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
            {
                selectedTab = 1;

                //Add Eraser
                LevelEditorBase.AddEraser();
                SelectItemModel(LevelEditorBase.AddNewItemModel());
                SaveItem();
            }
        }

        GUILayout.EndHorizontal();
    }

    private void SelectItemModel(LEItemModel selectedItemModel)
    {
        this.selectedItemModel = selectedItemModel;

        selectedItemName = selectedItemModel.itemName;
        selectedItemType = selectedItemModel.itemType;
    }

    private void ShowSelectedItemModelInfo()
    {
        EditorGUILayout.BeginHorizontal();

        selectedItemModel.itemIcon = (Sprite)EditorGUILayout.ObjectField
        (
            selectedItemModel.itemIcon,
            typeof(Sprite),
            false,
            GUILayout.Width(75),
            GUILayout.Height(75)
        );

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Name:", new GUILayoutOption[] { GUILayout.Width (50) });
        selectedItemName = EditorGUILayout.TextField(selectedItemName, new GUILayoutOption[] { GUILayout.Width(100) });

        GUILayout.Space(10);

        EditorGUILayout.LabelField("Type:", new GUILayoutOption[] { GUILayout.Width (50) });
        selectedItemType = (LEItemType)EditorGUILayout.EnumPopup(selectedItemType, new GUILayoutOption[] { GUILayout.Width (115) });

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Update", new GUILayoutOption[] { GUILayout.Width(200), GUILayout.Height(20) }))
        {
            selectedItemModel.itemName = selectedItemName;
            selectedItemModel.itemType = selectedItemType;

            SaveItem();
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void SaveItem()
    {
        if(selectedItemModel != null)
            LevelEditorBase.UpdateModel(selectedItemModel);

        EditorUtility.SetDirty(LevelEditorBase.gameObject);
        AssetDatabase.SaveAssets();
    }
    #endregion

    #region MapEditor
    private void GUIMap()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(60);
        GUILayout.BeginVertical();

        GUILayout.Label("MAP:", EditorStyles.label);

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        
        GUILayout.Label("ROWS:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(100), GUILayout.Height(25) });
        int rowCountBackup = rowCount;
        rowCount = EditorGUILayout.IntField(rowCount, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(25) });
        if(rowCount > maxRowColumnCount) rowCount = maxRowColumnCount;

        GUILayout.Space(10);

        GUILayout.Label("COLUMNS:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(100), GUILayout.Height(25) });
        int columnCountBackup = columnCount;
        columnCount = EditorGUILayout.IntField(columnCount, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(25) });
        if(columnCount > maxRowColumnCount) columnCount = maxRowColumnCount;

        GUILayout.Space(30);

        if(GUILayout.Button("Clear Gamefield", new GUILayoutOption[] {GUILayout.Width(100), GUILayout.Height(25) } ))
        {
            for (int i = 0; i < levelSquares.Length; i++)
            {
                levelSquares[i] = 0;
            }

            SaveLevel();
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        if(rowCountBackup != rowCount || columnCountBackup != columnCount) SaveLevel();
    }
    #endregion

    #region GameField
    private void GUIGameField()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(60);
        GUILayout.BeginVertical();

        GUILayout.Label("GAME FIELD:", EditorStyles.label);

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GenerateField();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void GenerateField()
    {
        if(rowCount > 0 && columnCount > 0)
        {
            GUILayout.BeginVertical();
            for(int i=0; i < rowCount; i++)
            {
                GUILayout.BeginHorizontal();
                for(int j=0; j < columnCount; j++)
                {
                    if(levelSquares[i * columnCount + j] != 0)
                    {
                        if(GUILayout.Button(LevelEditorBase.ItemModels[levelSquares[i * columnCount + j]].itemIcon.texture, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
                        {
                            if(levelSquares[i * columnCount + j] != brush)
                            {
                                SetType(j, i);
                            }
                        }

                    } else 
                    {
                        if(GUILayout.Button("", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
                        {
                            if(levelSquares[i * columnCount + j] != brush)
                            {
                                SetType(j, i);
                            }
                        }
                    }

                    GUI.backgroundColor = Color.white;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }

    void SetType(int col, int row)
    {
        levelSquares[row * columnCount + col] = brush;
        SaveLevel();
    }
    #endregion

    private int GetLastLevel()
    {
        TextAsset mapText = null;

        for (int i = levelNumber; i < 50000; i++)
        {
            mapText = Resources.Load("LevelData/" + i) as TextAsset;

            if (mapText == null)
            {
                return i - 1;
            }
        }
        
        return 0;
    }

    private void SaveLevel()
    {
        if (!fileName.Contains(".txt"))
            fileName += ".txt";
        
        SaveMap(fileName);
    }

    public void SaveMap(string fileName)
    {
        string saveString = "";
        //Create save string
        saveString += "MOVE COUNT " + (int)moveCount;
        saveString += "\r\n";
        saveString += "ROWS " + (int)rowCount;
        saveString += "\r\n";
        saveString += "COLUMNS " + (int)columnCount;
        saveString += "\r\n";

        //set map data
        for (int row = 0; row < rowCount; row++)
        {
            saveString += "MAPITEM";
            for (int col = 0; col < columnCount; col++)
            {
                saveString += (int)levelSquares[row * columnCount + col];
                //if this column not yet end of row, add space between them
                if (col < (columnCount - 1))
                    saveString += " ";
            }
            //if this row is not yet end of row, add new line symbol between rows
            if (row < (rowCount - 1))
                saveString += "\r\n";
        }

        //Write to file
        string resourcesDir = Application.dataPath + @"/Resources/";
        if(!Directory.Exists(resourcesDir))
                Directory.CreateDirectory(resourcesDir);

        string activeDir = resourcesDir + "LevelData/";
        if(!Directory.Exists(activeDir))
                Directory.CreateDirectory(activeDir);

        string newPath = System.IO.Path.Combine(activeDir, levelNumber + ".txt");

        StreamWriter sw = new StreamWriter(newPath);
        sw.Write(saveString);
        sw.Close();

        AssetDatabase.Refresh();
    }
    
    public bool LoadDataFromLocal(int currentLevel)
    {
        ResetTheVariables();

        //Read data from text file
        TextAsset mapText = Resources.Load("LevelData/" + currentLevel) as TextAsset;
        if (mapText == null)
            return false;
        
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
                moveCount = int.Parse(moveCountString);
            }
            else if (line.StartsWith("ROWS "))
            {
                string rowsString = line.Replace("ROWS", string.Empty).Trim();
                rowCount = int.Parse(rowsString);
            }
            else if (line.StartsWith("COLUMNS "))
            {
                string columnsString = line.Replace("COLUMNS", string.Empty).Trim();
                columnCount = int.Parse(columnsString);
            }
            else if(line.StartsWith("MAPITEM"))
            { //Maps
            //Split lines again to get map numbers
                string str = line.Replace("MAPITEM", string.Empty);
                string[] st = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < st.Length; i++)
                {
                    levelSquares[mapLine * columnCount + i] = int.Parse(st[i].ToString());
                }
                mapLine++;
            }
        }
    }

    private void ResetTheVariables()
    {
        moveCount = 1;
        rowCount = 1;
        columnCount = 1;

        levelSquares = new int[400];
    }
}
#endif

