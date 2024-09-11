open System
open System.IO

let parseHistoryLine (line: string) =
    // Check if the line starts with ':', indicating the extended format
    if line.StartsWith(":") then
        let command = line.Split([| ':'; ';' |], 4) |> Array.last
        let bareCommand = command.Split([| ' ' |]) |> Array.head
        bareCommand
    else
        ""

let commandsByFrequency count historyFile =
    // Reads the lines in the file
    File.ReadLines(historyFile)
    // Filter out blank line
    |> Seq.filter (fun line -> (String.length line) > 0)
    // Extract the command from the line
    |> Seq.map (fun line -> (parseHistoryLine line))
    // Group by command
    |> Seq.groupBy id
    // Count occurrences
    |> Seq.map (fun (command, group) -> (command, Seq.length group))
    // Sort by the count in descending order
    |> Seq.sortByDescending snd
    // Take the top `count` elements
    |> Seq.take count


[<EntryPoint>]
let main argv =
    let args = Environment.GetCommandLineArgs()

    // Print all the arguments
    printfn "Command-line arguments: %A" args

    // If there are specific arguments, print them
    match args with
    | [| _app; firstArg |] ->
        printfn "History file: %s" firstArg
        let result = commandsByFrequency 5 firstArg

        for (cmd, count) in result do
            printfn "Command: %s \tCount: %d" cmd count

    | _ -> printfn "Usage: dotnet run <firstArg>"

    0
