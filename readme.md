# Open Locker API Documentation

This API is used for Open Locker app which is an app to store your files on 
the cloud. Its like Google Drive but you can choose to store your files on 
any server you like or prefer. Currently Code is written to only support Azure
Blob storage but the architecture fully allows to use any type of storage to 
be used instead of it.

This project uses Custom Authentication implemented inside the 
OpenLockerWebApi app itself. It utilizes JWT tokens to register and allow users to access the authenticated endpoints.