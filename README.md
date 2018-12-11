## efrei-2018-zarwin

Pour build l'application :
`docker build . -t zarwin`

Lancer la base de donnée MongoDB : 
`docker run -d --name=mongo -i -t -p 27017:27017 mongo`

Puis lancer l'application console :
`docker run --link mongo:mongo zarwin`
