## efrei-2018-zarwin

Pour build l'application console :
`docker build -f app.Dockerfile . -t zarwin-app`

Pour build l'API :
`docker build -f api.Dockerfile . -t zarwin-api`

Lancer la base de donnée MongoDB : 
`docker run -d --name=mongo -i -t -p 27017:27017 mongo`

Puis lancer l'application console :
`docker run --link mongo:mongo zarwin-app`

Puis lancer l'api :
`docker run --link mongo:mongo zarwin-api`