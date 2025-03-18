# Gestura

## Introduction
**Gestura** is a .NET MAUI application designed to help artists and drawing enthusiasts **have fun** while improving their **gesture drawings** (quick sketches). The idea is simple: you select reference images, and the app automatically switches to the next image after a set amount of time. It’s a great way to **enjoy** varying poses and sharpen your drawing skills!

### Key Features
- **Set the time between each pose**: Easily adjust the interval (a few seconds or minutes) before the next image appears.  
- **Choose your images**:  
  - Import from your device’s gallery or local storage.  
  - Paste an image URL to download it directly.  
- **Create multiple sessions**: Manage different sessions (image lists, duration, etc.) for a **personalized experience** and even more **fun**.

### Who is it for?
- Artists (professionals or amateurs), art students…  
- Anyone looking to **spice up** their drawing sessions or simply **have fun** sketching poses on the fly.

---

## Installation
1. **Prerequisites**:  
   - .NET 7 (or a .NET MAUI–compatible version)  
   - Visual Studio 2022 (or equivalent) with the “.NET Multi-platform App UI development” workload  
2. **Clone the repository**:  
   ```bash
   git clone https://github.com/your-username/Gestura.git
   cd Gestura
3. **Restore NuGet packages**:
  ```bash
  dotnet restore
4. **Run the application**:
  - **Using Visual Studio**: Open the .sln file, then press F5.
  - **Using the command line**:
    ```bash
    dotnet build
    dotnet run
  - **Select your desired platform** (Android, iOS, Windows…).

---

## Usage
1. **Import images**:
  - **From your device**: Browse your folders to add your favorite pictures.
  - **From a URL**: Simply copy the link of the image and paste it into Gestura.
2. **Create a session**: Give it a name and set the interval duration between each pose.
3. **Start the session**: The slideshow begins! The app will display each image according to your chosen interval.
4. **Interaction**: Pause if you need more time, or skip an image if you’re eager to see the next one.

---

## Roadmap / Future Developments
- **Import from cloud drives** (Google Drive, Dropbox, etc.) for direct access to your collections.
- **Integration with inspiration platforms** (Pinterest, Tumblr, etc.).
- **Categorization / Tagging**: Organize your images by themes (landscapes, portraits, anatomy…).
- **Interface customization**: Color themes, light/dark mode, etc.
- **Statistics**: Keep track of the number of poses you’ve drawn, total drawing time… to celebrate your progress!
- **Internationalization**: Allow the app to adapt to the device’s language.
- **User Preferences**: Manage user settings within the app.

---

## Final Remarks
- **Technologies**: .NET MAUI (C#), cross-platform compatible (Android, iOS, Windows…).
- **Goal**: Make your drawing sessions more fun, more dynamic, and most importantly… more creative!

Enjoy Gestura, and let your imagination run wild!
