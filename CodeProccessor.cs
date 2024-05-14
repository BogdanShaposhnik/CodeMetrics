namespace CodeMetricsUtility
{
    class CodeMetricsProcessor
    {
        private string directory;
        private int linesOfCodeCount;
        private int emptyLinesCount;
        private int physicalLinesCount;
        private int logicalLinesCount;
        private int commentLinesCount;
        private int nestingLevel;
        private int kloc;

        public CodeMetricsProcessor(string directory)
        {
            this.directory = directory;
        }

        public void CalculateMetrics()
        {
            linesOfCodeCount = 0;
            emptyLinesCount = 0;
            physicalLinesCount = 0;
            logicalLinesCount = 0;
            commentLinesCount = 0;
            nestingLevel = 0;
            kloc = 0;

            ParseDirectory(directory);

            kloc = logicalLinesCount / 1000;

            Console.WriteLine("Code Metrics:");
            Console.WriteLine("Total lines of code: {0}", linesOfCodeCount);
            Console.WriteLine("Empty lines count: {0}", emptyLinesCount);
            Console.WriteLine("Physical lines count: {0}", physicalLinesCount);
            Console.WriteLine("Logical lines count: {0}", logicalLinesCount);
            Console.WriteLine("Commented lines Count: {0}", commentLinesCount);
            Console.WriteLine("Comment nesting level: {0}", nestingLevel);
            Console.WriteLine("KLOC: {0}", kloc);
        }

        private void ParseDirectory(string directory)
        {
            string[] files = Directory.GetFiles(directory, "*.cs");
            foreach (var file in files)
            {
                ProcessFile(file);
            }

            string[] subdirectories = Directory.GetDirectories(directory);
            foreach (var subdirectory in subdirectories)
            {
                ParseDirectory(subdirectory);
            }
        }

        private void ProcessFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            int emptyLines = 0;
            int physicalLines = 0;
            int commentLines = 0;
            bool commentFlag = false;
            int commentNestingLevel = 0;

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                physicalLines++;

                if (string.IsNullOrEmpty(trimmedLine))
                {
                    emptyLines++;
                    continue;
                }

                if (commentFlag)
                {
                    commentLines++;

                    if (trimmedLine.Contains("*/"))
                    {
                        if (nestingLevel < commentNestingLevel)
                        {
                            nestingLevel = commentNestingLevel;
                        }

                        commentNestingLevel--;
                        commentFlag = false;
                    }

                    continue;
                }

                if (trimmedLine.Contains("/*"))
                {
                    commentNestingLevel++;
                    commentLines++;

                    if (!trimmedLine.Contains("*/"))
                    {
                        commentFlag = true;
                    }

                    continue;
                }

                if (trimmedLine.StartsWith("//"))
                {
                    commentLines++;
                    continue;
                }
            }

            int logicalLines = physicalLines - commentLines - emptyLines;

            emptyLinesCount += emptyLines;
            physicalLinesCount += physicalLines;
            logicalLinesCount += logicalLines;
            linesOfCodeCount += logicalLines + emptyLines;
            commentLinesCount += commentLines;

            if (commentNestingLevel > nestingLevel)
            {
                nestingLevel = commentNestingLevel;
            }
        }
    }
}
