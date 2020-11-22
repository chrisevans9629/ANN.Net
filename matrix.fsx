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
        Fail (sprintf "expected %A but got %A" b a)
    else
        Pass

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

let dot a b =
    let (ar,ac) = size a
    let (br,bc) = size b
    

let matrixTest a b =
    test (getValue a 0 0) (Some b)

printfn "%A" (matrixTest (add A B) 7)
printfn "%A" (matrixTest (negate A) -3)
printfn "%A" (matrixTest (subtract A B) -1)
printfn "%A" (matrixTest (scalar A 10) 30)

let tran = transpose A

printfn "%A" (matrixTest (Some tran) 3)
printfn "%A" (test (getValue (Some tran) 0 1) (Some 4))


printfn "%A" (transpose A)