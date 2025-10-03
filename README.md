ConsoleAudioRecorder
Project Overview

ConsoleAudioRecorder is a lightweight C# console application designed to record audio from your microphone, store the audio files directly in a MySQL database, and play back the stored recordings. This project serves as a practical example of integrating audio processing and database management in a .NET console environment.

Using the powerful NAudio
 library, the application captures audio input in WAV format. The recorded audio is saved as binary data (LONGBLOB) in a MySQL database, making it easy to manage multiple recordings without dealing with filesystem storage.

This project showcases clean software architecture by separating responsibilities into layers: audio services for recording and playback, and data access layer for database interactions. The user interacts with the app through a simple console menu to record new audio, list existing recordings, and play any selected audio clip.

How It Works
1. Recording Audio

When the user chooses to record, the app uses NAudio's WaveInEvent to capture audio from the default microphone.

Audio data is streamed into a memory buffer (MemoryStream) and saved as a WAV format file in memory.

The recording continues until the user presses any key to stop.

The recorded audio bytes are then saved into the MySQL database as a LONGBLOB in the AudioTable.

2. Saving Audio to Database

The app uses ADO.NET with the MySql.Data connector to interact with MySQL.

The binary audio data is inserted into the AudioTable with a timestamp (RecordedAt).

The table schema consists of:

Id: Auto-increment primary key

AudioData: Audio stored as a binary large object (BLOB)

RecordedAt: Timestamp of when the recording was made

3. Listing Stored Recordings

When the user opts to list recordings, the app queries the database for all saved audio entries.

It displays a list of recording IDs along with their timestamps.

This allows users to see all available recordings and pick one to play.

4. Playing Back Audio

After selecting a recording by its ID, the app retrieves the binary audio data from the database.

Using NAudio's WaveFileReader and WaveOutEvent, the app plays the audio directly from memory.

Playback continues until the audio finishes, providing basic console feedback.

Project Structure

Program.cs: Handles user input and menu navigation.

Services/AudioService.cs: Contains methods for recording audio and playing audio clips.

DataLayer/AudioRepository.cs: Contains methods to save, load, and list audio recordings in the MySQL database.

Utils/IgnoreDisposeStream.cs: Helper stream class to manage stream disposal during recording.

Prerequisites

.NET 6 SDK
 or later

MySQL Server
 with a configured database

NuGet packages:

NAudio

MySql.Data

Setup Instructions

Create the MySQL table

CREATE TABLE AudioTable (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    AudioData LONGBLOB,
    RecordedAt DATETIME
);


Update your connection string

In AudioRepository.cs, replace the placeholder connection string with your MySQL credentials:

private readonly string connectionString = "Server=localhost;Database=your_db;User ID=your_user;Password=your_password;";


Build and run

Use the following commands in your project directory:

dotnet build
dotnet run

Usage

When the app starts, you get a menu:

1. Record and Save
Starts recording from the microphone. Press any key to stop recording and save the audio.

2. List and Play Audio
Displays all saved recordings with their IDs and timestamps. Choose an ID to play the audio.

Why This Project?

This project provides a real-world example of:

Recording and playing audio using NAudio in a .NET console application.

Storing large binary data (audio) efficiently in a relational database.

Implementing a clean architecture with separation of concerns.

Handling user input and basic UI interaction in a console app.

Future Enhancements (Ideas)

Add support for deleting recordings from the database.

Export audio clips from the database to WAV files on disk.

Add metadata like audio length, format, or custom names.

Implement asynchronous database and audio operations.

Add user authentication for multi-user audio management.
