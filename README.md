# 3-semester-EksamensOpgave

## Portmapping

| Service | Adresse på værten | Containerport | Protokol |
| --- | --- | ---: | --- |
| `VidensVejen-Frontend-Admin` | `localhost:8090` | `80` | HTTP |
| `VidensVejen-Frontend-Admin` | `localhost:8091` | `443` | HTTPS |
| `VidensVejen-Frontend-Kiosk` | `localhost:8190` | `80` | HTTP |
| `VidensVejen-Frontend-Kiosk` | `localhost:8191` | `443` | HTTPS |
| `mssql-UserDb` | `127.0.0.1:9001` | `1433` | SQL Server |
| `mssql-AuthUserDb` | `127.0.0.1:9002` | `1433` | SQL Server |
| `mssql-ExhibitionDb` | `127.0.0.1:9003` | `1433` | SQL Server |
| `mssql-GameDb` | `127.0.0.1:9004` | `1433` | SQL Server |
| `mssql-StatisticsDb` | `127.0.0.1:9005` | `1433` | SQL Server |
| `mssql-ImageDb` | `127.0.0.1:9006` | `1433` | SQL Server |

Frontendportene er publiceret på alle værtsmaskinens netværksinterfaces. SQL Server-portene er bundet til `127.0.0.1` og kan derfor kun tilgås fra selve værtsmaskinen via de viste værtsporte.

YARP-proxyen og API-services lytter på port `8080` internt i Docker. Kafka bruger `kafka-broker:9092` internt. Compose-filen publicerer ingen værtsporte for disse services.