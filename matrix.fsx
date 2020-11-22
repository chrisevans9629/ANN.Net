let A = [
    [3;8]
    [4;6]
]

let B = [
    [4;0]
    [1;-9]
]

type TestResult = Pass | Fail of string

let test a b =
    if not (a.Equals(b)) then
        printfn "%A" (Fail (sprintf "expected %A but got %A" b a))
    else
        printfn "%A" Pass

//printfn "%A" (test 1 1)
//printfn "%A" (test 1 2)

let size a =
    (a |> List.length, a |> List.head |> List.length)    

let getValue a r c =
    match a with
    | Some m -> Some (m |> List.item r |> List.item c)
    | None -> None
let add a b =
    let (ar,ac) = size a
    let (br,bc) = size b
    if ar <> br || ac <> bc then
        None
    else
        Some [for row in 0..ar-1 -> [for column in 0..ac-1 -> a.[row].[column] + b.[row].[column]]]

let negate a =
    Some [for y in a -> [for x in y -> -x]]

let subtract a b =
    let nb = negate b
    match nb with
    | Some b -> add a b
    | None -> None

let scalar a s =
    Some [for y in a -> [for x in y -> x * s]]

let transpose a =
    let (ro,co) = size a
    [for c in 0..co-1 ->[for r in 0..ro-1 -> a.[r].[c]]]

//[1,3]x[1
//       2]
//
let dot a b =
    let (ar,ac) = size a
    let (br,bc) = size b
    let mul a b r c =
        let row: int list = a |> List.item r 
        let column: int list = [for row in b -> row |> List.item c]
        [for i in 0..row.Length-1 -> row.[i] * column.[i]] |> List.sum

    if ac <> br then 
        printfn "%A did not match %A" (ar,ac) (br, bc)
        None
    else
        Some [for r in 0..ar-1 -> [for c in 0..bc-1 -> mul a b r c]]

let identity size =
    [for r in 0..size-1 -> [for c in 0..size-1 -> if r = c then 1 else 0]]

let inverse a =
    a

let div a b =
    dot (a) (inverse b)

let matrixTest a b =
    test (getValue a 0 0) (Some b)

matrixTest (add A B) 7
matrixTest (negate A) -3
matrixTest (subtract A B) -1
matrixTest (scalar A 10) 30

let tran = transpose A

matrixTest (Some tran) 3
test (getValue (Some tran) 0 1) (Some 4)

let dotResult = dot A B
printfn "%A" dotResult
matrixTest dotResult 20

let X = [
    [3;4;2]
]

let Y = [
    [13;9;7;15]
    [8;7;4;6]
    [6;4;0;3]
]

printfn "%A" (dot X Y)

matrixTest (dot [[1;2;3]] [[4];[5];[6]]) 32

let idM = identity 3

test (size idM) (3,3)
test (size X) (1, 3)
printfn "%A" idM

printfn "%A" (dot X idM)
//printfn "%A" (dot idM X)

//matrixTest (dot idM X) 3
//printfn "%A" (transpose A)