# Movie Rating API

An API based on ASP.Net Core 3.1 and Entity Framework Core for retreiving and adding Movie ratings.

# Get Movies based on Filter criteria

Curl 
curl -X GET "https://localhost:5001/api/MovieRating?MovieName=Guardian" -H  "accept: */*"

Request URL
https://localhost:5001/api/MovieRating?MovieName=Guardian

# Get Top 5 Movies based on total average user rating

Curl 
curl -X GET "https://localhost:5001/api/MovieRating/top" -H  "accept: */*"

Request URL
https://localhost:5001/api/MovieRating/top

# Get Top 5 rated Movies by a specific user

Curl 
curl -X GET "https://localhost:5001/api/MovieRating/top/Raj" -H  "accept: */*"

Request URL
https://localhost:5001/api/MovieRating/top/Raj

# Add or Update user rating for a movie

Curl
curl -X POST "https://localhost:5001/api/MovieRating" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"movieName\":\"The Matrix\",\"userName\":\"Raj\",\"rating\":3}"

Request URL
https://localhost:5001/api/MovieRating
