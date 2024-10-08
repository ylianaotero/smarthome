-- CREAR USUARIOS
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-07 22:51:02.095', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR3qCM3wTZo3M9KpWjlUrWfKKBroM33YzNpmg&s', N'Emily', N'Gilmore', N'Password1%', N'emilygilmore@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-07 22:53:53.888', N'https://static.wikia.nocookie.net/gilmoregirls/images/e/e7/3richard.jpeg/revision/latest?cb=20170623151348', N'Richard', N'Gilmore', N'Password1%', N'richardgilmore@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-07 22:55:45.983', NULL, N'Lorelai', N'Gilmore', N'Password1%', N'lorelaigilmore@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-07 22:57:51.702', N'https://i.pinimg.com/736x/d1/60/b2/d160b24e1ab2cacacc479a158bb0db33.jpg', N'Rory', N'Gilmore', N'Password1%', N'rorygilmore@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-08 01:03:01.538', N'https://i.pinimg.com/736x/f2/1c/7f/f21c7fbf754a4a077f8cc3ce61e1ce2f.jpg', N'Sookie', N'StJames', N'Password1%', N'sookiestjames@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-08 02:01:18.320', NULL, N'Luke', N'Danes', N'Password1%', N'lukedanes@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-08 02:11:14.640', N'https://static.wikia.nocookie.net/gilmoregirls/images/5/5d/Paris.jpeg/revision/latest?cb=20160408232615', N'Paris', N'Geller', N'Password1%', N'parisgeller@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-08 09:32:34.851', NULL, N'Lane', N'Kim', N'Password1%', N'lanekim@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-08 09:38:29.840', N'https://static.wikia.nocookie.net/gilmoregirls/images/7/7c/312mrskim.png/revision/latest?cb=20160803202749', N'Emily', N'Kim', N'Password1%', N'emilykim@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-08 15:56:53.402', N'https://i.pinimg.com/originals/38/85/f1/3885f159655c6e99f3639352d93f4330.jpg', N'Jess', N'Mariano', N'Password1%', N'jessmariano@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-08 15:57:49.020', N'https://www.newamericanjackets.com/wp-content/uploads/2020/01/Dean-Forester-Gilmore-Girls-Jared-Padalecki-Jacket.jpg', N'Dean', N'Forester', N'Password1%', N'deanforester@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-08 15:58:24.677', N'https://i.pinimg.com/736x/2a/53/5c/2a535c6f74e23cff6dc250536532a537.jpg', N'Tristan', N'Dugray', N'Password1%', N'tristandugray@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-10-08 16:04:08.355', NULL, N'Kirk', N'Gleason', N'Password1%', N'kirkgleason@gmail.com');


-- CREAR ROLES
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'Administrator', 1, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'HomeOwner', 2, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'CompanyOwner', 3, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'Administrator', 4, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'HomeOwner', 5, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'CompanyOwner', 6, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'HomeOwner', 7, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'Administrator', NULL, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'CompanyOwner', 8, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'HomeOwner', 9, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'Administrator', 10, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'Administrator', 11, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'Administrator', 12, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'CompanyOwner', 13, 0, NULL);


-- CREAR EMPRESAS
INSERT INTO sqlserver.dbo.Companies (OwnerId, Name, RUT, LogoURL) VALUES(3, N'Dragonfly Inn', N'765432109', N'https://www.vectorkhazana.com/assets/images/products/Dragonfly.png');
INSERT INTO sqlserver.dbo.Companies (OwnerId, Name, RUT, LogoURL) VALUES(6, N'Amazon IoT', N'57349854728', N'https://play-lh.googleusercontent.com/tXi5rTVQdi3Nk24wdUKOED1NA0ovw6GsZmfLSjZQNRmLUDtdAPVaas7NujI9Pc4ttrU');
INSERT INTO sqlserver.dbo.Companies (OwnerId, Name, RUT, LogoURL) VALUES(8, N'Kim Devices', N'345365346', N'https://play-lh.googleusercontent.com/tXi5rTVQdi3Nk24wdUKOED1NA0ovw6GsZmfLSjZQNRmLUDtdAPVaas7NujI9Pc4ttrU');

-- AGREGAR ID DE EMPRESAS A ROLES
UPDATE sqlserver.dbo.Roles SET Kind=N'CompanyOwner', UserId=3, HasACompleteCompany=1, CompanyId=1 WHERE Id=3;
UPDATE sqlserver.dbo.Roles SET Kind=N'CompanyOwner', UserId=6, HasACompleteCompany=1, CompanyId=2 WHERE Id=9;
UPDATE sqlserver.dbo.Roles SET Kind=N'CompanyOwner', UserId=8, HasACompleteCompany=1, CompanyId=3 WHERE Id=13;


-- CREAR DISPOSITIVOS
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, LocationType, Functionalities) VALUES(N'Google Nest Outdoor Wired Security Standard Surveilance Camera', 2400, N'Google Nest Outdoor Wired Security Standard Surveilance Camera (2 Pack) - NC2400ES Bundle with Deco Gear 2 Pack WiFi Smart Plug (4 Items)', N'["https://m.media-amazon.com/images/I/61peqnk1FSL._AC_SL1000_.jpg","https://m.media-amazon.com/images/I/61Vz\u002BoifEuL._AC_SL1000_.jpg","https://m.media-amazon.com/images/I/5166StS2CBL._AC_SL1000_.jpg"]', 1, N'SecurityCamera', 1, N'[1]');
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, LocationType, Functionalities) VALUES(N'Arlo Pro 5S Spotlight Security Camera 2K HDR', 2262, N'Arlo Pro 5S Spotlight Security Camera 2K HDR | Outdoor | Wire-Free with Spotlight | Dual-Band Wi-Fi Connects to the Strongest Network | 12X Zoom, 2-Way Audio, Color Night Vision, Live Stream White', N'["https://m.media-amazon.com/images/I/51UrwPSLzIL._AC_SL1498_.jpg","https://m.media-amazon.com/images/I/81rrite7v9L._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/814B59zS4EL._AC_SL1500_.jpg"]', 1, N'SecurityCamera', 1, N'[]');
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, LocationType, Functionalities) VALUES(N'Arlo Essential Security Camera 2K', 8281, N'Arlo Essential Security Camera 2K | Indoor - Outdoor | 2nd Gen | Wireless with Spotlight, 2-Way Audio, Color Night Vision, Live Stream, Motion Activiation, Real Time Notifications - White, 2 Camera', N'["https://m.media-amazon.com/images/I/71i1zax59rL._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/81-9sKp-I2L._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/91fOtLc0QxL._AC_SL1500_.jpg"]', 1, N'SecurityCamera', 1, N'[1]');
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, LocationType, Functionalities) VALUES(N'Smart WiFi Door Sensor for Home', 780, N'This door sensor is an excellent security addition to any contact structure: front door, window, baby room, garage doors, skylights, safes, etc. Watching the security barrier at the top of the stairs. All the statuses of doors and windows will be displayed simultaneously in the app. No hub required. 2 AAA Batteries no included.', N'["https://m.media-amazon.com/images/I/61Tjp9wsjzL._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/61lNBXHaE2L._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/61kpM\u002B7dWRL._AC_SL1200_.jpg"]', 1, N'WindowSensor', NULL, N'[0]');
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, LocationType, Functionalities) VALUES(N'WiFi Door or Window Alarm System', 8065, N'This door sensor is an excellent security addition to any contact structure: front door, window, baby room, garage doors, skylights, safes, etc. Watching the security barrier at the top of the stairs. All the statuses of doors and windows will be displayed simultaneously in the app. No hub required. 2 AAA Batteries no included.', N'["https://m.media-amazon.com/images/I/71e3gJyiIBL._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/61wpgkpEcxL._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/61wpgkpEcxL._AC_SL1500_.jpg"]', 1, N'WindowSensor', NULL, N'[0]');
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, LocationType, Functionalities) VALUES(N'Ring Alarm Contact Sensor', 8065, N'Ring Alarm Contact Sensor (2nd Gen)', N'["https://m.media-amazon.com/images/I/41xYb7UXP1L._SL1000_.jpg","https://m.media-amazon.com/images/I/41siPqHKP1L._SL1000_.jpg","https://m.media-amazon.com/images/I/51edNlMRAjL._SL1000_.jpg"]', 2, N'WindowSensor', NULL, N'[0]');
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, LocationType, Functionalities) VALUES(N'Blink Mini', 4834, N'Compact indoor plug-in smart security camera, 1080p HD video, human and motion detection, two-way audio, easy set up, Works with Alexa – 2 cameras (White)', N'["https://m.media-amazon.com/images/I/51vncWY4ROL._SL1000_.jpg","https://m.media-amazon.com/images/I/31k0hI-y8pL.jpg","https://m.media-amazon.com/images/I/41iQH85g1MS.jpg"]', 2, N'SecurityCamera', 0, N'[0,1]');
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, LocationType, Functionalities) VALUES(N'Ring Alarm Contact Sensor', 8065, N'Ring Alarm Contact Sensor (2nd Gen)', N'["https://m.media-amazon.com/images/I/41xYb7UXP1L._SL1000_.jpg","https://m.media-amazon.com/images/I/41siPqHKP1L._SL1000_.jpg","https://m.media-amazon.com/images/I/51edNlMRAjL._SL1000_.jpg"]', 3, N'WindowSensor', NULL, N'[0]');
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, LocationType, Functionalities) VALUES(N'Ring Indoor Cam (newest model)', 482943674, N'Home or business security in 1080p HD video. Place it on a table. Stick it on a wall. Switch it up. With the versatile Indoor Cam, protection goes wherever there’s a plug. See more with HD video and Advanced Pre-Roll—even in the dark. Or see less with a manual Privacy Cover that turns off your cam until you need it next.', N'["https://m.media-amazon.com/images/I/61i8ASPWAxL._SL1000_.jpg","https://m.media-amazon.com/images/I/51keHnu-7YL._SL1500_.jpg","https://m.media-amazon.com/images/I/51kdSQdo\u002B1L._SL1000_.jpg","https://m.media-amazon.com/images/I/51WTT5smqyL._SL1000_.jpg"]', 3, N'SecurityCamera', 0, N'[0,1]');


-- CREAR HOGARES
INSERT INTO sqlserver.dbo.Homes (OwnerId, Street, DoorNumber, Latitude, Longitude, MaximumMembers, HomeOwnerId) VALUES(2, N'Pearl St', 233, 41.7637, 72.6851, 5, 2);
INSERT INTO sqlserver.dbo.Homes (OwnerId, Street, DoorNumber, Latitude, Longitude, MaximumMembers, HomeOwnerId) VALUES(5, N'Main St', 453, 25.0534, 75.2345, 10, 5);
INSERT INTO sqlserver.dbo.Homes (OwnerId, Street, DoorNumber, Latitude, Longitude, MaximumMembers, HomeOwnerId) VALUES(7, N'Second St', 385, 24.3452, 23.9348, 1, 7);
INSERT INTO sqlserver.dbo.Homes (OwnerId, Street, DoorNumber, Latitude, Longitude, MaximumMembers, HomeOwnerId) VALUES(9, N'Third St', 436, 547.458457, 23.6457, 1, 9);
INSERT INTO sqlserver.dbo.Homes (OwnerId, Street, DoorNumber, Latitude, Longitude, MaximumMembers, HomeOwnerId) VALUES(2, N'Fourth St', 457, 547.458457, 23.6457, 1, 2);


-- CREAR UNIDADES DE DISPOSITIVOS
INSERT INTO sqlserver.dbo.DeviceUnits (HardwareId, DeviceId, IsConnected, HomeId) VALUES(N'3C46BFC7-BD2B-4122-A1CE-0E43A3B8F1A6', 2, 1, 2);
INSERT INTO sqlserver.dbo.DeviceUnits (HardwareId, DeviceId, IsConnected, HomeId) VALUES(N'FF4431D6-4F12-4602-BF2C-0FA64470713B', 2, 0, 1);
INSERT INTO sqlserver.dbo.DeviceUnits (HardwareId, DeviceId, IsConnected, HomeId) VALUES(N'FD0D9CC5-8DFD-41FC-A64E-7CA8234A543D', 1, 1, 2);
INSERT INTO sqlserver.dbo.DeviceUnits (HardwareId, DeviceId, IsConnected, HomeId) VALUES(N'8F56AB9E-7CA9-4897-AE3B-AF5C36AE5920', 1, 1, 1);

-- CREAR MIEMBROS DE HOGAR
INSERT INTO sqlserver.dbo.Members (UserId, HasPermissionToListDevices, HasPermissionToAddADevice, ReceivesNotifications, HomeId) VALUES(1, 1, 0, 1, 1);
INSERT INTO sqlserver.dbo.Members (UserId, HasPermissionToListDevices, HasPermissionToAddADevice, ReceivesNotifications, HomeId) VALUES(4, 1, 0, 1, 1);
INSERT INTO sqlserver.dbo.Members (UserId, HasPermissionToListDevices, HasPermissionToAddADevice, ReceivesNotifications, HomeId) VALUES(5, 1, 0, 1, 1);
INSERT INTO sqlserver.dbo.Members (UserId, HasPermissionToListDevices, HasPermissionToAddADevice, ReceivesNotifications, HomeId) VALUES(8, 1, 0, 0, 1);
INSERT INTO sqlserver.dbo.Members (UserId, HasPermissionToListDevices, HasPermissionToAddADevice, ReceivesNotifications, HomeId) VALUES(8, 1, 0, 1, 2);


-- CREAR NOTIFICACIONES
INSERT INTO sqlserver.dbo.Notifications (HomeId, MemberId, DeviceUnitHardwareId, Event, CreatedAt, [Read], ReadAt) VALUES(1, 1, N'8F56AB9E-7CA9-4897-AE3B-AF5C36AE5920', N'Movement detected', '2024-10-08 09:04:35.479', 0, '0001-01-01 00:00:00.000');
INSERT INTO sqlserver.dbo.Notifications (HomeId, MemberId, DeviceUnitHardwareId, Event, CreatedAt, [Read], ReadAt) VALUES(1, 2, N'8F56AB9E-7CA9-4897-AE3B-AF5C36AE5920', N'Movement detected', '2024-10-08 09:04:35.575', 0, '0001-01-01 00:00:00.000');
INSERT INTO sqlserver.dbo.Notifications (HomeId, MemberId, DeviceUnitHardwareId, Event, CreatedAt, [Read], ReadAt) VALUES(1, 3, N'8F56AB9E-7CA9-4897-AE3B-AF5C36AE5920', N'Movement detected', '2024-10-08 09:04:35.581', 0, '0001-01-01 00:00:00.000');
INSERT INTO sqlserver.dbo.Notifications (HomeId, MemberId, DeviceUnitHardwareId, Event, CreatedAt, [Read], ReadAt) VALUES(2, 5, N'3C46BFC7-BD2B-4122-A1CE-0E43A3B8F1A6', N'Movement detected', '2024-10-08 09:49:08.819', 0, '0001-01-01 00:00:00.000');


-- CREAR SESIONES
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'EF02F7D5-5D82-4C03-9775-08DCE73BC90E', 1);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'EE4AE5D2-E167-46BB-9776-08DCE73BC90E', 2);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'AD718CC6-BC27-4719-9778-08DCE73BC90E', 2);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'F21FA3EA-4F6B-4E19-9777-08DCE73BC90E', 3);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'E9CDAFA5-16E8-4DAD-1C43-08DCE74F87C1', 4);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'5ADD637D-9C26-40E8-81DD-08DCE758EBF9', 5);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'84FFCEE3-60E8-4DEF-CD1D-08DCE7955BA3', 9);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'5F37265A-3717-4619-CD1F-08DCE7955BA3', 9);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'E7AD15DC-7DC9-4685-CD20-08DCE7955BA3', 9);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'69129D60-8D1E-49DC-CD1E-08DCE7955BA3', 10);