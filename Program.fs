open System
open System.IO

let parseHistoryLine (line: string) =
    if line.StartsWith(":") then
        let semiColonIndex = line.IndexOf(';')
        // 1. Get the command i.e. everything after the semicolon
        let fullCommand = line.Substring(semiColonIndex + 1)
        // 2. Split by space to get the first part of the command
        let command = fullCommand.Split([| ' ' |]) |> Array.head
        Some command
    else
        None

let commandsByFrequency count historyFile =
    // Returns an enumerable over the lines in the file
    File.ReadLines(historyFile)
    // Take only the non-blank lines
    |> Seq.choose (fun line -> if String.IsNullOrWhiteSpace line then None else Some line)
    // Extract the command from the line
    |> Seq.choose (fun line -> (parseHistoryLine line))
    // Group by command -> (command, seq of commands)
    |> Seq.groupBy id
    // Count occurrences
    |> Seq.map (fun (command, occurrences) -> (command, Seq.length occurrences))
    // Sort by the count in descending order
    |> Seq.sortByDescending snd
    // Take the top `count` elements
    |> Seq.take count


[<EntryPoint>]
let main argv =
    let args = Environment.GetCommandLineArgs()

    match args with
    | [| _app; historyFile |] ->
        printfn "History file: %s" historyFile
        let result = commandsByFrequency 10 historyFile

        for (cmd: string, count) in result do
            printfn "Command: %s \tCount: %d" cmd count

    | _ -> printfn "Usage: dotnet run <historyFile>"

    0
