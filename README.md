# TaskManager

This application is a task management system. 
# That allows:
- Add Employees
- Update Employees
- Remove Employees
- Create Tasks
- Manage Tasks
- Update Tasks
- Remove Tasks
- Inform the employee, when he/she was assigned/ unsigned from the task

# How To Start?
1. Pull the repository
2. Install .net 7 sdk
3. Install docker desktop
4. This application uses Mailgun service, log in on Mailgun, and receive your apiKEY and all other necessary information.
5. Update the Mailgun section in the appsettings.Production.json in NotificationService/TaskManager.Notification.API
6. Build docker images. In your terminal ensure, that you are in the TaskManager folder, then run commands, one by one:
- docker build -t task-service -f TaskService/Dockerfile .
- docker build -t team-service -f TeamService/Dockerfile .
- docker build -t notification-service -f NotificationService/Dockerfile .
- docker build -t api-gateway -f Gateway/Dockerfile .
7. Run docker-compose up
8. After all containers are started, you can use Postman for a test. I attached the postman collection

Application Architecture

<img src="https://github.com/TarasIskiv/TaskManager/assets/66842006/0b1b2655-858e-41ee-9471-a37021b0b91c" alt="mudblazor" width="1200" height="500"/> 

The application starts 9 containers. With 2 separate database containers and 2 separate containers for Redis cache.
The application has configured volumes for databases, caches and RabbitMQ.
Also, for each microservice, there is configured its own network. In sum, there are 3 different networks
