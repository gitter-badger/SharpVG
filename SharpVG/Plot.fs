﻿namespace SharpVG

type Plot =
    {
        Elements: seq<Element>
        Minimum: Point
        Maximum: Point
        Title: string option
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Plot =

    let create minimum maximum elements =
        {Elements = elements; Minimum = minimum; Maximum = maximum; Title = None}

    let line values =
        let (minimum, maximum) =
            let (xValues, yValues) = values |> List.unzip
            ((xValues |> List.min, yValues |> List.min), (xValues |> List.max, xValues |> List.max))

        let elements = 
            values
            |> List.scan (fun acc p -> acc |> (Path.addAbsolute PathType.LineTo (Point.ofFloats p))) Path.empty
            |> List.toSeq
            |> Seq.map Element.ofPath

        create (Point.ofFloats minimum) (Point.ofFloats maximum) elements

    let toGroup plot =
        plot.Elements
        |> Seq.map (Element.withStyle { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Pixel 1.0); Fill = None; Opacity = None })
        |> Group.ofSeq
        |> Group.asCartesian plot.Minimum.X plot.Maximum.Y

