using System.Collections.Generic;

public class WorldNode : BaseNode
{
    public List<MapNode> mapNodes;

    void Awake()
    {
        NodeType = NodeTypes.WorldNode;
    }

}
