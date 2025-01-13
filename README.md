# DesignSphere
dotnet ef --startup-project ..\DesignSphere.Api\ migrations add Initial --context AppDbContext
dotnet ef --startup-project ..\DesignSphere.Api\ migrations add Initial2 --context SQLContext
dotnet ef --startup-project ..\DesignSphere.Api\ database update


## How to run services
## Prerequisites
Before running the services, ensure you have the following tools installed:
- **Docker**: To run Docker containers. [Install Docker](https://docs.docker.com/get-docker/)
- **Docker Compose**: To orchestrate multi-container Docker applications. [Install Docker Compose](https://docs.docker.com/compose/install/)
-.net build 
- net run
-rabbit and postgres
-sdk .net6 and .net 8


## Schema
The schema of the database is as follows:

- [ ] Issues
	- [ ] TaskID,Taskdescription
	- [ ] conversations
		- [ ] sender ,reciver,messagetype,messgaecontent 
- [ ] Project
	- [ ] collaborators==>owner,devloper,viewer
	- [ ] Tasks
		- [ ] ProjectId,status==>(completed,incompleted,Notstarted),name,description,deadline,taskweight
		- [ ] TaskAssignees
			- [ ] TaskstageId,collaboratorid ,status==>(completed,incompleted,Notstarted)
		-[ ] TaskDetail
			- [ ] TaskId,Name==>Todo,Designing,Review,status ==> (completed,incompleted,Notstarted),Projectstageid

-ProjectStage
	- [ ] order,name,description
		

## Features
- [ ] Auth Module
	- [x] Sign-in
	- [x] Sign-up
		- [x] Email Verifcation
		- [x] Profile Registration 
	- [x] Password flow
		- [x] Forgot password
		- [x] Reset password
	- [x] 2fa Authentication
		- [x] Registration
			- [x] Google
			- [x] Email
		- [x] Verification
			- [x] Google verification
			- [x] Email Verification
		- [x] Troubleshoot 2fa
			- [x] Google
			- [x] Email
	- [x] Delete Account
	- [x] Logout 
	- [x] Email Service Added
- [ ] Project Module
	- [x] ProjectController
		- [x] Create Project
		- [x] GetAll Project  
	- [x] ContributerController
		- [x] Add Contributer
		- [x] Get Contributer
	- [x]  PlanController
		- [x] GetProject Plans
	- [x] DoorController
		- [x] Create DoorsOpenings
	- [] Notes
	- [] Activity Feeds

- [x] Misc Changes
	- [x] Domain Schema Updated
	- [x] sieve custom integrated for pagination
	- [x] BuildingBlocks updated