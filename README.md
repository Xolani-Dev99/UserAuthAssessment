# UserAuthAssessment

Full-stack authentication system built with:

- Frontend: React
- Backend: .NET Web API
- Database: PostgreSQL
- Containerization: Docker (API + PostgreSQL)
- Testing: xUnit

------------------------------------------------------------

PROJECT STRUCTURE

UserAuthAssessment/
|
|-- user-auth-frontend/        React frontend
|-- UserAuthAPI/               .NET Web API
|-- UserAuthAPI.Tests/         Backend test project
|-- docker-compose.yml
|-- README.md

------------------------------------------------------------

REQUIREMENTS

Make sure you have installed:

- Docker Desktop
- Node.js (v18+ recommended)
- npm
- Git

------------------------------------------------------------

HOW TO RUN THE APPLICATION

1. Clone the repository

git clone https://github.com/Xolani-Dev99/UserAuthAssessment.git
cd UserAuthAssessment


2. Start Backend + PostgreSQL (Docker)

From the root folder:

docker-compose up --build

This starts:
- .NET Web API
- PostgreSQL database

The API will be available at:
http://localhost:5000


3. Run the React Frontend

Open a NEW terminal window and run:

cd user-auth-frontend
npm install
npm start

The frontend will run at:
http://localhost:3000


------------------------------------------------------------

HOW IT CONNECTS

- React runs locally on port 3000
- The .NET API runs in Docker on http://localhost:5000
- PostgreSQL runs inside Docker
- The API connects to PostgreSQL using the Docker service name

React uses:
http://localhost:5000/api/


------------------------------------------------------------

RUNNING TESTS

cd UserAuthAPI.Tests
dotnet test


------------------------------------------------------------

STOPPING THE APPLICATION

To stop containers:

docker-compose down

To remove volumes (if needed):

docker-compose down -v


------------------------------------------------------------

NOTES FOR REVIEWERS

- Backend and database are fully containerized.
- Frontend runs locally for development simplicity.
- Authentication includes registration and login.
- Protected routes require authentication.
- Backend includes unit tests.

------------------------------------------------------------

FUTURE IMPROVEMENTS

- Containerize React for full Docker setup
- Add refresh tokens
- Add role-based authorization
- Improve test coverage
