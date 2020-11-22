// #r "nuget: MathNet.Numerics.FSharp"

// open MathNet.Numerics.LinearAlgebra

// let m = matrix [[1.0; 2.0;];[3.0;4.0]]
#load "Matrix.fsx"
open Matrix

test 1 1
type Cell<'a> = {Value:'a;Positions:int list}


// let zeros<'a> (dim:int list) (v:'a) =
//     let cellCount = dim |> List.fold (fun x y -> x * y)
//     v
