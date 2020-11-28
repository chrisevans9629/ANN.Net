module Test
open Matrix

type TestResult = Pass | Fail of string

let test name a b =
    if not (a.Equals(b)) then
        printfn "%A: %A" name (Fail (sprintf "expected %A but got %A" b a))
    else
        printfn "%A: %A" name Pass

let matrixTest name (a: Matrix option) b =
    test name (a |> Option.map (fun r -> getValue r 0 0)) (Some b)

matrixTest "Add-Op" (A + B) 7.
matrixTest "Add" (add A B) 7.
matrixTest "Negate" (Some (negate A)) -3.
matrixTest "Subtract" (subtract A B) -1.
matrixTest "Scalar" (Some (scalar A 10.)) 30.

let tran = transpose A

matrixTest "Tran" (Some tran) 3.
test "TranVal" (getValue (tran) 0 1) (Some 4.)

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

let ran = flatten (random 1 10)
printfn "%A" (ran)