* Requirements
- Docker

"docker-compose up -d" command must be run from the cli for run the project over docker.

When the project was run over Docker, we would automatically create the relevant database, complete the migration and make the first data entries.

The following endpoint should be used to start the vehicle distribute.
-POST "http://localhost:8080/v1/Vehicles/{Vehicle}/Distribute"

The following endpoint should be used for the project swagger.
-GET "http://localhost:8080/swagger/index.html"

The following endpoint should be used for the project health check.
-GET "http://localhost:8080/health"

The following endpoint and model should be used to view the vehicle distribution logs.
-POST "http://localhost:9200/fleet_log_pool/_search"
{
  "size": 1000,
  "query": {
    "match": {
      "eventId": "1234"
    }
  }
}
