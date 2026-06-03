# JSON Configuration Tagger

A WPF utility for updating one or more JSON configuration files by appending custom configuration values.

Built with:

* .NET
* WPF
* MVVM Community Toolkit
* Newtonsoft.Json

---

# Features

* Select multiple JSON files
* Scan folders for JSON files
* Preview JSON content
* Add custom configuration entries
* Apply updates to all selected files
* Validate JSON before processing
* Batch processing support
* MVVM architecture

---

# Use Case

This tool is designed to quickly update application configuration files such as:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  }
}
```

by adding entries like:

```json
{
  "GlobalConfigPath": "D:\\Config\\worker.global.appsettings.json"
}
```

Result:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "GlobalConfigPath": "D:\\Config\\worker.global.appsettings.json"
}
```

---

# Getting Started

## Build Requirements

* Visual Studio 2022 or later
* .NET SDK 8.0 or later

Restore packages:

```powershell
dotnet restore
```

Build:

```powershell
dotnet build
```

Run:

```powershell
dotnet run
```

---

# Usage

## 1. Select Files

Click:

```
Select Files
```

Choose one or more JSON files.

Supported examples:

```text
appsettings.json
appsettings.Development.json
worker.json
custom.json
```

---

## 2. Select Folder

Click:

```
Select Folder
```

The application will recursively search for:

```text
*.json
```

within the selected directory.

---

## 3. Add Tags

Click:

```
Add Tag
```

Fill in:

| Field | Description         |
| ----- | ------------------- |
| Key   | Configuration key   |
| Value | Configuration value |

Example:

| Key              | Value                                    |
| ---------------- | ---------------------------------------- |
| GlobalConfigPath | D:\Config\worker.global.appsettings.json |

---

## 4. Preview

Click:

```
Preview
```

The selected file content will be displayed in the Preview panel.

---

## 5. Apply Changes

Click:

```
Apply Changes
```

The application will update all loaded JSON files.

---

# Example

Tag:

```text
Key:
GlobalConfigPath

Value:
D:\Config\worker.global.appsettings.json
```

Generated JSON:

```json
{
  "GlobalConfigPath": "D:\\Config\\worker.global.appsettings.json"
}
```

---

# Project Structure

```text
JsonTagger
│
├── Models
│   ├── JsonFileModel.cs
│   ├── JsonTagModel.cs
│
├── Services
│   ├── JsonFileService.cs
│   ├── DialogService.cs
│
├── ViewModels
│   ├── MainViewModel.cs
│
├── Views
│   ├── MainWindow.xaml
│
└── App.xaml
```

---

# Architecture

The application follows the MVVM pattern.

### View

Responsible for UI rendering.

```text
MainWindow.xaml
```

### ViewModel

Handles commands and state.

```text
MainViewModel
```

### Services

Handles:

* File selection
* JSON loading
* JSON updates

---

# Common Issues

## InitializeComponent Does Not Exist

Verify:

```xml
<UseWPF>true</UseWPF>
```

exists in the project file.

Clean and rebuild:

```text
bin/
obj/
```

folders.

---

## JSON Validation Failed

The selected file contains invalid JSON.

Validate the file before processing.

---

## No Files Loaded

Verify:

* Files exist
* File extension is `.json`
* User has read permissions

---

# Future Enhancements

* Material Design UI
* Drag & Drop support
* Dark / Light theme
* Backup file creation
* Progress bar
* Export processing report
* Undo changes
* JSON diff viewer

 
