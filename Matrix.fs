module Matrix

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

let matrix a =
    {Data=a}

//printfn "%A" (test 1 1)
//printfn "%A" (test 1 2)

let size (a:Matrix) =
    (a.Data |> List.length, a.Data |> List.head |> List.length)    

let getValue (a:Matrix) r c =
    (a.Data |> List.item r |> List.item c)

let setValue (a:Matrix) row column value =
    {Data=[for r in 0..a.Data.Length-1 -> [for c in 0..a.Data.[r].Length-1 -> if r = row && c = column then value else a.Data.[r].[c] ]]}

let merge (a:Matrix) (b:Matrix) (func:float -> float -> float):Matrix option =
    let (ar,ac) = size a
    let (br,bc) = size b
    if ar <> br || ac <> bc then
        None
    else
        Some (matrix [for row in 0..ar-1 -> 
                        [for column in 0..ac-1 -> 
                            func (getValue a row column) (getValue b row column) ]])

let add (a:Matrix) (b:Matrix): Matrix option =
    merge a b (fun x y -> x + y)

let mult a b =
    merge a b (fun x y -> x * y)

let mult2 a b =
    a |> Option.map (fun r -> mult r b) |> Option.flatten

let negate (a:Matrix) =
    {Data=[for y in a.Data -> [for x in y -> -x]]}

let subtract a b =
    let nb = negate b
    add a nb

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

let map (a:Matrix) func =
    {Data=[for r in a.Data -> [for c in r -> func c]]}


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
    static member (+) (a,b) =
        map a (fun x -> x + b)
    static member (-) (a,b) =
        subtract a b
    static member (-) (a,b) =
        map a (fun x -> x - b)
    static member (-) (a,b) =
        map b (fun x -> a - x)
    
    static member (*) (a,b) =
        mult a b
    static member (*) (a,b) =
        mult2 a b
    //static member (*) (a,b) =
    //    match a with
    //    | Some aa -> dot aa b
    //    | None -> None
    //static member (*) (a,b) =
    //    dot a b
    static member (*) (a,b) =
        scalar a b
    static member (*) (a,b) =
        scalar b a
    static member (*) (a,b) =
        match b with
        | Some bb -> Some (scalar bb a)
        | None -> None
    static member (/) (a,b) =
        scalar a (1./b)
    member x.T() =
        transpose x



