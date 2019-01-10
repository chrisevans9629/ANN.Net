namespace Tests

open NUnit.Framework
open ANN.Core.Net

open NeuralNetworks
[<TestClass>]
type TestClass () =
    let network = CreateNetwork (fun()->1.) (fun(r)->r) [2;3;2]
    

    let testNetworkSize network =


        
        Assert.AreEqual(2, network.Layers.[0].Nodes.Length)
        Assert.AreEqual(3, network.Layers.[1].Nodes.Length)
        Assert.AreEqual(2, network.Layers.[2].Nodes.Length)
        Assert.AreEqual(2, network.NetworkConnections.Length)
        Assert.AreEqual(6, network.NetworkConnections.[0].Connections.Length)
        Assert.AreEqual(6, network.NetworkConnections.[1].Connections.Length)
        
        Assert.AreEqual(network.Layers.[0], network.NetworkConnections.[0].Input)
        Assert.AreEqual(network.Layers.[0].Nodes.[0], network.NetworkConnections.[0].Connections.[0].Input)
    [<SetUp>]
    member this.Setup () =
        ()
    
    [<Test>]
    member this.``Create Network``()=
        testNetworkSize network
        ()
    
    [<Test>]
    member this.``set the inputs``()=
        let inputsSet = SetInputs network [1.;1.]
        testNetworkSize inputsSet
        ()
    [<Test>]
    member this.``Calculate The First Layer``()=
        let inputset = SetInputs network [1.;1.]
        let firstLayer = CalculateLayer inputset.NetworkConnections.[0] inputset.Normalization
        Assert.AreEqual(2,firstLayer.Nodes.[0].NodeValue)
        ()


    [<Test>]
    member this.``Calculate Network``()=
        let newNetwork = CalculateNetwork network [1.;1.]
        testNetworkSize newNetwork
        ()
