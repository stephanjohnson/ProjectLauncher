## Project Launcher

Project Launcher is a utility that helps with managing multiple projects or solutions in modern software development. It can scan a directory for all the .csproj and .sln files and allows you to quickly spin up running copies of the code directly from the command line, while you only focus on having an IDE open for the project you are actively working on.

### Features

Scan a directory for all .csproj and .sln files
Launch console and web apps using dotnet run
Open Visual Studio with the solution file

### Installation

The easiest way to install the tool is to use dotnet tool install:

``` bash
dotnet tool install --global ProjectLauncher
```

### Usage
To use Project Launcher, simply open a command prompt or terminal and navigate to the directory where your projects are located. Then, run the following command:

#### Launch Web Apps
``` bash
project web
```

This will scan the directory for all .csproj files that are web apps and present you with a list of projects to choose from. You can then select one or more projects to run.

#### Launch Console Apps
``` bash
project console
```

This will scan the directory for all .csproj files that are console apps and present you with a list of projects to choose from. You can then select one or more projects to run.

#### Open Solution in Visual Studio
``` bash
project sln
```

This will scan the directory for all .sln files and present you with a list of solutions to choose from. You can then select one or more solutions to open with Visual Studio.

For more information, you can run the following command:

bash
Copy code
project --help
Contributing
Contributions are welcome! If you find a bug or have an idea for a new feature, please open an issue on GitHub. If you would like to contribute code, please fork the repository and submit a pull request.

License
Project Launcher is licensed under the MIT License.
