//#r "nuget: FSharp.Charting"

type MatrixData = float list list
type Matrix = {
    Data:MatrixData
    }


let A = {Data =[
        [3.;8.]
        [4.;6.]
    ]}

let B = {Data=[
        [4.;0.]
        [1.;-9.]
    ]}


//printfn "%A" (test 1 1)
//printfn "%A" (test 1 2)

let size (a:Matrix) =
    (a.Data |> List.length, a.Data |> List.head |> List.length)    

let getValue (a:Matrix option) r c =
    match a with
    | Some m -> Some (m.Data |> List.item r |> List.item c)
    | None -> None
let add a b: Matrix option =
    let (ar,ac) = size a
    let (br,bc) = size b
    if ar <> br || ac <> bc then
        None
    else
        Some {Data=[for row in 0..ar-1 -> [for column in 0..ac-1 -> a.Data.[row].[column] + b.Data.[row].[column]]]}

let negate (a:Matrix) =
    Some {Data=[for y in a.Data -> [for x in y -> -x]]}

let subtract a b =
    let nb = negate b
    match nb with
    | Some b -> add a b
    | None -> None

let scalar a s =
    {Data=[for y in a.Data -> [for x in y -> x * s]]}

let transpose a =
    let (ro,co) = size a
    {Data=[for c in 0..co-1 ->[for r in 0..ro-1 -> a.Data.[r].[c]]]}

let dot a b: Matrix option =
    let (ar,ac) = size a
    let (br,bc) = size b
    let mul a b r c =
        let row: float list = a |> List.item r 
        let column: float list = [for row in b -> row |> List.item c]
        [for i in 0..row.Length-1 -> row.[i] * column.[i]] |> List.sum

    if ac <> br then 
        printfn "%A did not match %A" (ar,ac) (br, bc)
        None
    else
        Some {Data=[for r in 0..ar-1 -> [for c in 0..bc-1 -> mul a.Data b.Data r c]]}

let identity size =
    {Data=[for r in 0..size-1 -> [for c in 0..size-1 -> if r = c then 1. else 0.]]}

let zeroes rows columns =
    {Data=[for r in 0..rows-1 -> [for c in 0..columns-1 -> 0.0]]}

let rand = new System.Random()

let random rows columns =
    {Data=[for r in 0..rows-1 -> [for c in 0..columns-1 -> rand.NextDouble() * 2. - 1.]]}

let flatten (matrix: Matrix) =
    [for r in 0..matrix.Data.Length-1 do 
        for c in 0..matrix.Data.[r].Length-1 -> matrix.Data.[r].[c]]
// let inverse a =
//     a

//let div a b =
//     dot (a) (inverse b)

 
//OPERATORS

type Matrix with
    static member (+) (a,b) =
        add a b
    static member (*) (a,b) =
        dot a b
    static member (*) (a,b) =
        scalar a b
    static member (/) (a,b) =
        scalar a (1./b)
// let inline (+) a b =
//     add a b
// let inline (*) a b =
//     dot a b
// let inline (*) a b =
//     scalar a b
// let inline (-) a b =
//     subtract a b
// let inline (/) a b =
//     scalar a (1./b)

type TestResult = Pass | Fail of string

let test name a b =
    if not (a.Equals(b)) then
        printfn "%A: %A" name (Fail (sprintf "expected %A but got %A" b a))
    else
        printfn "%A: %A" name Pass

let matrixTest name a b =
    test name (getValue a 0 0) (Some b)

matrixTest "Add-Op" (A + B) 7.
matrixTest "Add" (add A B) 7.
matrixTest "Negate" (negate A) -3.
matrixTest "Subtract" (subtract A B) -1.
matrixTest "Scalar" (Some (scalar A 10.)) 30.

let tran = transpose A

matrixTest "Tran" (Some tran) 3.
test "TranVal" (getValue (Some tran) 0 1) (Some 4.)

let dotResult = dot A B
printfn "%A" dotResult
matrixTest "Dot" dotResult 20.



let X = {Data=[
    [3.;4.;2.]
    ]}

let Y = {Data=[
    [13.;9.;7.;15.]
    [8.;7.;4.;6.]
    [6.;4.;0.;3.]
    ]}
let matrix a =
    {Data=a}
printfn "%A" (dot X Y)

matrixTest "Dot2" (dot (matrix [[1.;2.;3.]]) (matrix [[4.];[5.];[6.]])) 32.

let idM = identity 3

test "IdSize" (size idM) (3,3)
test "IdSize" (size X) (1, 3)
printfn "%A" idM

printfn "%A" (dot X idM)

//printfn "%A" (zeros [3, 2])
//printfn "%A" (dot idM X)

//matrixTest (dot idM X) 3
//printfn "%A" (transpose A)

test "Zeroes" (size (zeroes 4 5)) (4,5)

open System
let ran = flatten (random 1 10)
printfn "%A" (ran)
let sigmoid (x:float): float = 1. + 1.//1. / (1. +  Math.Pow(Math.E, -x))
//open FSharp.Charting
//#r "nuget: System.Windows.Forms.DataVisualization"

//Chart.Histogram(ran)
// self.inodes = inputnodes
//         self.hnodes = hiddennodes
//         self.onodes = outputnodes
//         self.lr = learningrate
//         self.wih = numpy.random.normal(0, pow(self.inodes, -0.5), (self.hnodes, self.inodes))
//         self.who = numpy.random.normal(0, pow(self.hnodes, -0.5), (self.onodes, self.hnodes))
//         self.activation_function = lambda x: scipy.special.expit(x)
type Network = {
    inputsNodes:int
    hiddenNodes:int
    outputNodes:int
    learning:float
    weightsInputHidden:Matrix
    weightsHiddenOutput:Matrix
    activationFunction: float -> float
    }

let network i h o lr =
    {
        inputsNodes = i
        hiddenNodes = h
        outputNodes = o
        learning = lr
        activationFunction = sigmoid
        weightsInputHidden = (random h i) / float(i)
        weightsHiddenOutput = (random o h) / float(h)
    }