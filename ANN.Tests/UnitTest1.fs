namespace Tests

open NUnit.Framework
open ANN.Core.Net

open NeuralNetworks
[<TestClass>]
type TestClass () =

    [<SetUp>]
    member this.Setup () =
        ()
    
    [<Test>]
    member this.``Create Network``()=
        
        let network = CreateDefaultNetwork [2;3;2]

        Assert.AreEqual(2, network.Layers.[0].Nodes.Length)
        Assert.AreEqual(3, network.Layers.[1].Nodes.Length)
        Assert.AreEqual(2, network.Layers.[2].Nodes.Length)
        Assert.AreEqual(2*3+3*2, network.Connections.Length)
        ()


    [<Test>]
    member this.Test1 () =
        Assert.Pass()
