using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class NodeBasedEditor : EditorWindow
{
    private List<Node> nodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    // Rect for buttons to clear, save and load
    private Rect rectButtonClear;
    private Rect rectButtonSave;
    private Rect rectButtonLoad;

    // Count for nodes created
    private int nodeCount;

    // Where we store the skilltree
    private UpgradeTree upgradeTree;

    // Dictionary with the skills in our skilltree
    private Dictionary<int, Upgrade> upgradeDictionary;

    private EditorZoomer zoomer = new EditorZoomer();

    [MenuItem("Window/Upgrade Tree Editor")]
    private static void OpenWindow()
    {
        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Upgrade Tree Editor");
    }

    private void OnEnable()
    {
        upgradeTree = new UpgradeTree();

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

        // Create Buttons for clear, save and load
        rectButtonClear = new Rect(new Vector2(10, 10), new Vector2(60, 20));
        rectButtonSave = new Rect(new Vector2(80, 10), new Vector2(60, 20));
        rectButtonLoad = new Rect(new Vector2(150, 10), new Vector2(60, 20));

        // Initialize nodes with saved data
        LoadNodes();
    }

    private void OnGUI()
    {
        //zoomer.Begin();

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        // Draw the Buttons
        DrawButtons();

        DrawNodes();
        DrawConnections();

        DrawConnectionLine(Event.current);

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();

        //zoomer.End();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++) {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++) {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
        if (nodes != null) {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null) {
            for (int i = 0; i < connections.Count; i++) {
                connections[i].Draw();
            }
        }
    }

    // Draw out new buttons for managing the skill tree
    private void DrawButtons()
    {
        if (GUI.Button(rectButtonClear, "Clear"))
            ClearNodes();
        if (GUI.Button(rectButtonSave, "Save"))
           SaveSkillTree();
        if (GUI.Button(rectButtonLoad, "Load"))
            LoadNodes();
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    ClearConnectionSelection();
                }
                if (e.button == 1) {
                    ProcessContextMenu(e.mousePosition);
                }
                break;
            case EventType.MouseDrag:
                if (e.button == 0 || e.button == 2) {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null) {
            for (int i = nodes.Count - 1; i >= 0; i--) {
                bool guiChanged = nodes[i].ProcessEvents(e);

                if (guiChanged) GUI.changed = true;
            }
        }
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null) {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null) {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (nodes != null) {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        if (nodes == null)
            nodes = new List<Node>();

        nodes.Add(new Node(mousePosition, 300, 200, nodeStyle, selectedNodeStyle,
            inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,
            nodeCount, false, 0));
        ++nodeCount;
    }

    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null) {
            if (selectedOutPoint.node != selectedInPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
            } else {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null) {
            if (selectedInPoint.node != selectedOutPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
            } else {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveNode(Node node)
    {
        if (connections != null) {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++) {
                if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint) {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++) {
                connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        nodes.Remove(node);
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (connections == null)
            connections = new List<Connection>();

        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    private void ClearNodes()
    {
        nodeCount = 0;
        if (nodes != null && nodes.Count > 0) {
            Node node;
            while (nodes.Count > 0) {
                node = nodes[0];
                OnClickRemoveNode(node);
            }
        }
    }

    // Save the data from the window to the skill tree
    private void SaveSkillTree()
    {
        if (nodes.Count > 0) {
            // We fill with as many skills as nodes we have
            upgradeTree.upgradeTree = new Upgrade[nodes.Count];
            int[] dependencies;
            List<int> dependenciesList = new List<int>();

            // Iterate over all of the nodes. Populating the skills with the node info
            for (int i = 0; i < nodes.Count; ++i) {
                if (connections != null) {
                    List<Connection> connectionsToRemove = new List<Connection>();
                    List<ConnectionPoint> connectionPointsToCheck = new List<ConnectionPoint>();

                    for (int j = 0; j < connections.Count; j++) {
                        if (connections[j].inPoint == nodes[i].inPoint) {
                            for (int k = 0; k < nodes.Count; ++k) {
                                if (connections[j].outPoint == nodes[k].outPoint) {
                                    dependenciesList.Add(k);
                                    break;
                                }
                            }
                            connectionsToRemove.Add(connections[j]);
                            connectionPointsToCheck.Add(connections[j].outPoint);
                        }
                    }
                }

                dependencies = dependenciesList.ToArray();
                dependenciesList.Clear();
                upgradeTree.upgradeTree[i] = nodes[i].upgrade;
                upgradeTree.upgradeTree[i].upgradeDependencies = dependencies;
            }

            string json = JsonUtility.ToJson(upgradeTree);
            string path = null;

            path = "Assets/Data/upgradeTree.json";

            // finally, we write the JSOn string with teh SkillTree data in our file
            using (FileStream fs = new FileStream(path, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(fs)) {
                    writer.Write(json);
                }
            }

            UnityEditor.AssetDatabase.Refresh();

            SaveNodes();
        }
    }

    private void SaveNodes()
    {
        NodeDataCollection nodeData = new NodeDataCollection();
        nodeData.nodeDataCollection = new NodeData[nodes.Count];

        for (int i = 0; i < nodes.Count; ++i) {
            nodeData.nodeDataCollection[i] = new NodeData();
            nodeData.nodeDataCollection[i].nodeID = nodes[i].upgrade.upgradeID;
            nodeData.nodeDataCollection[i].position = nodes[i].rect.position;
        }

        string json = JsonUtility.ToJson(nodeData);
        string path = "Assets/Data/upgradeTreeNodeData.json";

        using (FileStream fs = new FileStream(path, FileMode.Create)) {
            using (StreamWriter writer = new StreamWriter(fs)) {
                writer.Write(json);
            }
        }

        UnityEditor.AssetDatabase.Refresh();
    }

    private void LoadNodes()
    {
        ClearNodes();

        string dataPath = "Assets/Data/upgradeTreeNodeData.json";
        string dataAsJson;
        NodeDataCollection loadedData;

        if (File.Exists(dataPath)) {

            dataAsJson = File.ReadAllText(dataPath);

            loadedData = JsonUtility.FromJson<NodeDataCollection>(dataAsJson);

            Upgrade[] _upgradeTree;
            List<Upgrade> originNode = new List<Upgrade>();
            upgradeDictionary = new Dictionary<int, Upgrade>();
            string path = "Assets/Data/upgradeTree.json";
            Vector2 pos = Vector2.zero;

            if (File.Exists(path)) {
                // Read the json from the file into a string
                dataAsJson = File.ReadAllText(path);

                // Pass the JSON to JsonUtility, and tell it to create a SkillTree object from it
                UpgradeTree upgradeData = JsonUtility.FromJson<UpgradeTree>(dataAsJson);

                // Store the SkillTree as an array of Skill
                _upgradeTree = new Upgrade[upgradeData.upgradeTree.Length];
                _upgradeTree = upgradeData.upgradeTree;

                // Create Nodes
                for (int i = 0; i < _upgradeTree.Length; ++i) {

                    for (int j = 0; j < loadedData.nodeDataCollection.Length; ++j) {
                        if (loadedData.nodeDataCollection[j].nodeID == _upgradeTree[i].upgradeID) {
                            pos = loadedData.nodeDataCollection[j].position;
                            break;
                        }
                    }
                    LoadSkillCreateNode(_upgradeTree[i], pos);

                    if (_upgradeTree[i].upgradeDependencies.Length == 0) {
                        originNode.Add(_upgradeTree[i]);
                    }

                    upgradeDictionary.Add(_upgradeTree[i].upgradeID, _upgradeTree[i]);
                }

                Upgrade outUpgrade;
                Node outNode;
                // Create connections
                for (int i = 0; i < nodes.Count; ++i) {
                    for (int j = 0; j < nodes[i].upgrade.upgradeDependencies.Length; ++j) {
                        if (upgradeDictionary.TryGetValue(nodes[i].upgrade.upgradeDependencies[j], out outUpgrade)) {
                            for (int k = 0; k < nodes.Count; ++k) {
                                if (nodes[k].upgrade.upgradeID == outUpgrade.upgradeID) {
                                    outNode = nodes[k];
                                    OnClickOutPoint(outNode.outPoint);
                                    break;
                                }
                            }
                            OnClickInPoint(nodes[i].inPoint);
                        }
                    }
                }
            }
        } else {
            Debug.LogError("Cannot load game data");
        }
    }

    private void LoadSkillCreateNode(Upgrade skill, Vector2 position)
    {
        if (nodes == null) {
            nodes = new List<Node>();
        }

        nodes.Add(new Node(position, 300, 200, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, skill.upgradeID, skill.upgradeName, skill.upgradeDescription, skill.unlocked, skill.cost, skill.upgradeDependencies));
        ++nodeCount;
    }
}
