using QuizApi.Models;

namespace QuizApi.Data
{
    public static class QuizData
    {
        public static readonly Dictionary<string, List<Question>> Questions =
            new Dictionary<string, List<Question>>
        {
            {
                "HTML", new List<Question>
                {
                    new Question
                    {
                        QuestionText = "What does HTML stand for?",
                        Options = new List<string>
                        {
                            "Hyper Text Markup Language",
                            "Home Tool Markup Language",
                            "Hyperlinks and Text Markup Language",
                            "Hyperlinking Text Managing Language"
                        },
                        Answer = "Hyper Text Markup Language"
                    },
                    new Question
                    {
                        QuestionText = "Which HTML element is used for the largest heading?",
                        Options = new List<string> { "<heading>", "<h6>", "<h1>", "<head>" },
                        Answer = "<h1>"
                    },
                    new Question
                    {
                        QuestionText = "What is the correct HTML element for inserting a line break?",
                        Options = new List<string> { "<br>", "<lb>", "<break>", "<newline>" },
                        Answer = "<br>"
                    },
                    new Question
                    {
                        QuestionText = "Which attribute specifies an alternate text for an image?",
                        Options = new List<string> { "src", "title", "alt", "longdesc" },
                        Answer = "alt"
                    },
                    new Question
                    {
                        QuestionText = "What is the correct HTML element for playing video files?",
                        Options = new List<string> { "<media>", "<movie>", "<video>", "<player>" },
                        Answer = "<video>"
                    }
                }
            },
            {
                "CSS", new List<Question>
                {
                    new Question
                    {
                        QuestionText = "What does CSS stand for?",
                        Options = new List<string>
                        {
                            "Cascading Style Sheets",
                            "Colorful Style Sheets",
                            "Creative Style Syntax",
                            "Computer Styled Sheets"
                        },
                        Answer = "Cascading Style Sheets"
                    },
                    new Question
                    {
                        QuestionText = "Which property changes the text color of an element?",
                        Options = new List<string> { "font-color", "color", "text-color", "fgcolor" },
                        Answer = "color"
                    },
                    new Question
                    {
                        QuestionText = "How do you make each word start with a capital letter?",
                        Options = new List<string>
                        {
                            "text-transform: uppercase;",
                            "text-style: capitalize;",
                            "transform: capitalize;",
                            "text-transform: capitalize;"
                        },
                        Answer = "text-transform: capitalize;"
                    },
                    new Question
                    {
                        QuestionText = "Which property controls the space between lines of text?",
                        Options = new List<string>
                        {
                            "line-height",
                            "letter-spacing",
                            "word-spacing",
                            "spacing"
                        },
                        Answer = "line-height"
                    },
                    new Question
                    {
                        QuestionText = "Correct syntax to link external CSS file?",
                        Options = new List<string>
                        {
                            "<link rel='stylesheet' href='style.css'>",
                            "<style src='style.css'>",
                            "<css link='style.css'>",
                            "<link src='style.css'>"
                        },
                        Answer = "<link rel='stylesheet' href='style.css'>"
                    }
                }
            },
            {
                "JavaScript", new List<Question>
                {
                    new Question
                    {
                        QuestionText = "Which company developed JavaScript?",
                        Options = new List<string> { "Microsoft", "Sun Microsystems", "Netscape", "Oracle" },
                        Answer = "Netscape"
                    },
                    new Question
                    {
                        QuestionText = "Which keyword declares a variable in JavaScript?",
                        Options = new List<string> { "int", "let", "define", "declare" },
                        Answer = "let"
                    },
                    new Question
                    {
                        QuestionText = "What is the output of typeof null?",
                        Options = new List<string> { "null", "object", "undefined", "string" },
                        Answer = "object"
                    },
                    new Question
                    {
                        QuestionText = "Which symbol is used for comments?",
                        Options = new List<string> { "//", "/*", "#", "<!--" },
                        Answer = "//"
                    },
                    new Question
                    {
                        QuestionText = "Which method converts JSON to a JS object?",
                        Options = new List<string>
                        {
                            "JSON.convert()",
                            "JSON.toObject()",
                            "JSON.parse()",
                            "JSON.stringify()"
                        },
                        Answer = "JSON.parse()"
                    }
                }
            },
            {
                "TypeScript", new List<Question>
                {
                    new Question
                    {
                        QuestionText = "TypeScript is a superset of which language?",
                        Options = new List<string> { "C#", "Java", "JavaScript", "Python" },
                        Answer = "JavaScript"
                    },
                    new Question
                    {
                        QuestionText = "Which feature catches type errors at compile time?",
                        Options = new List<string> { "Type inference", "Static typing", "Dynamic typing", "Loose typing" },
                        Answer = "Static typing"
                    },
                    new Question
                    {
                        QuestionText = "Which keyword defines an interface?",
                        Options = new List<string> { "type", "struct", "interface", "define" },
                        Answer = "interface"
                    },
                    new Question
                    {
                        QuestionText = "File extension of TypeScript?",
                        Options = new List<string> { ".js", ".jsx", ".ts", ".tsx" },
                        Answer = ".ts"
                    },
                    new Question
                    {
                        QuestionText = "Which command compiles TS to JS?",
                        Options = new List<string> { "tsc", "npm start", "ts-run", "node-ts" },
                        Answer = "tsc"
                    }
                }
            }
        };
    }
}
