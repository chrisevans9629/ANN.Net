namespace ANN.Core.Net

module NeuralNetworks =
    open System
    open System.Linq
    //type Id = Guid
    type Node = {NodeValue:float;}
    type Weight = {WeightValue:float;}
    type Connection = {Input:Node;Weight:Weight;Output:Node}
    type NetworkLayer = {Nodes:Node[]}
    type NetworkConnection = {Input:NetworkLayer;Connections:Connection[];Output:NetworkLayer}
    type Network = {Layers:NetworkLayer list;NetworkConnections:NetworkConnection list;Generator:unit->float;Normalization:float->float}
    let Sigmoid x = x/(Math.Sqrt(1.0+x*x));

    let CreateNode x = {NodeValue=x;};

    //let CreateSigmoidNode x = CreateNode (Sigmoid x); 

    let CreateLayer values normalizationFunction = {Nodes=[|for x in values do yield CreateNode (normalizationFunction x)|]}

    let CreateLayerWithSize size = {Nodes=[|for x in [0..size-1] -> CreateNode 0.0|]};
    let CreateWeight x = {WeightValue=x;};
    let CreateConnection inputNode weight outputNode = {Input=inputNode;Weight=weight;Output=outputNode};

    let CreateConnections inputLayer outputLayer weight =
        [|for n1 in inputLayer.Nodes do for n2 in outputLayer.Nodes -> CreateConnection n1 (CreateWeight (weight())) n2|]

    let CreateNetworkConnection inputLayer outputLayer weight = 
        {Input=inputLayer;Output=outputLayer;Connections=CreateConnections inputLayer outputLayer weight}
    
    let GetConnections (layers:NetworkLayer list) weight =
        [for l in [0..layers.Length-2] -> CreateNetworkConnection layers.[l] layers.[l+1] weight] ;

    let CreateNetwork weight normalize layout =
        let layers = [for l in layout -> CreateLayerWithSize l];
        let connections = GetConnections layers weight
        {Layers=layers;NetworkConnections=connections;Normalization=normalize;Generator=weight}
    
    

    let random = new Random();
    let rand() = random.NextDouble() * 2.0 - 1.0;
    let CreateDefaultNetwork layout = CreateNetwork  rand Sigmoid layout;

    let CalculateLayer (connections:NetworkConnection) normalization =
        CreateLayer [for r in connections.Connections.GroupBy(fun c-> c.Output) -> r.Sum(fun s -> s.Input.NodeValue * s.Weight.WeightValue)] normalization
    

    let SetInputs network input =
        let inputLayer = CreateLayer input network.Normalization;
        let connection = {network.NetworkConnections.First() with Input = inputLayer};

        let layers = [for r in network.Layers.Skip(1) -> r] |> List.append  [inputLayer]
        let connections = [for r in network.NetworkConnections.Skip(1) -> r] |> List.append  [connection]
        let newNetwork = {network with Layers=layers; NetworkConnections=connections}
        newNetwork
    let CalculateNetwork network input =
        let newNetwork = SetInputs network input;
        let layers = [for r in newNetwork.NetworkConnections -> CalculateLayer r newNetwork.Normalization] |> List.append  [newNetwork.Layers.First()];
        {newNetwork with Layers=layers}
        
module Tests =
    open System
    
    let random = new Random();
    let rand() = random.NextDouble() * 2.0 - 1.0;
    let network = NeuralNetworks.CreateNetwork rand NeuralNetworks.Sigmoid [2;3;2];

    let NewNetwork = NeuralNetworks.CalculateNetwork network [1.;2.];
    
    
    