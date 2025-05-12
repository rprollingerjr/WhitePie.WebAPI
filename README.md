# WhitePie.WebAPI (.NET 8)

This is the official backend for the WhitePie project, built with ASP.NET Core 8, connected to MongoDB, and designed for flexible deployment to Railway or Azure.

---

## 🚀 Features

- Built with .NET 8
- MongoDB support via MongoDB.Driver
- GridFS for image uploads
- RESTful API for Events, Moments, Menu, Booking, About content
- Email notifications via Resend

---

## 🛠️ Local Development

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MongoDB (local or Atlas)
- `appsettings.json` configured properly

### Run the app
```bash
dotnet restore
dotnet run
