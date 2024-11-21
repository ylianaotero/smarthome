-- CREAR USUARIOS
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-11-21 00:41:49.399', N'https://static.wikia.nocookie.net/gilmoregirls/images/d/d7/Jackson_zpsbc778b3a.jpg/revision/latest?cb=20131229072506', N'Jackson', N'Belleville', N'Password1%', N'jacksonbelleville@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-11-21 00:45:04.916', N'https://i.pinimg.com/736x/f6/38/df/f638dfacbdfbd93376c36646499d2020.jpg', N'Michel', N'Gerard', N'Password1%', N'michelgerard@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-11-21 00:45:34.681', N'https://hellogiggles.com/wp-content/uploads/sites/7/2016/08/26/hBA3MpLwTiAx.jpg?quality=82&strip=all', N'Richard', N'Gilmore', N'Password1%', N'richardgilmore@gmail.com');
INSERT INTO sqlserver.dbo.Users (CreatedAt, Photo, Name, Surname, Password, Email) VALUES('2024-11-21 00:46:30.911', NULL, N'Dave', N'Rygalski', N'Password1%', N'drygalski@gmail.com');

-- CREAR ROLES
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'Administrator', 1, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'HomeOwner', 2, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'HomeOwner', 3, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'CompanyOwner', 4, NULL, NULL);
INSERT INTO sqlserver.dbo.Roles (Kind, UserId, HasACompleteCompany, CompanyId) VALUES(N'HomeOwner', 1, NULL, NULL);


-- CREAR EMPRESAS
INSERT INTO sqlserver.dbo.Companies (OwnerId, Name, RUT, LogoURL, ValidationMethod) VALUES(4, N'IoT Home', N'473457534', N'https://t4.ftcdn.net/jpg/03/85/50/33/360_F_385503388_tBGEcrjitRyBXbbDQVSH14BXacRvo0cX.jpg', N'VALIDATORLETTER');

-- AGREGAR ID DE EMPRESAS A ROLES
UPDATE sqlserver.dbo.Roles SET Kind=N'CompanyOwner', UserId=4, HasACompleteCompany=1, CompanyId=1 WHERE Id=4;


-- CREAR DISPOSITIVOS
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, Functionalities, LocationType) VALUES(N'Window Alarm Sensors 4 Pack', N'abcdef', N'Glass Break Sensor 130DB Loud Window Vibration Alarms, Burglar Intruder Entry Detector for Indoor Home Office Apartment & RV Security', N'["https://m.media-amazon.com/images/I/81KHpQqAfZL._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/81d3gWM2VHL._AC_SL1500_.jpg","https://m.media-amazon.com/images/I/71MS3oCQ34L._AC_SL1500_.jpg"]', 1, N'WindowSensor', N'[0]', NULL);
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, Functionalities, LocationType) VALUES(N'Echo Glow', N'abcdef', N'Multicolor smart lamp, Works with Alexa', N'["https://m.media-amazon.com/images/I/61yTkc3VJ1L._AC_SL1000_.jpg","https://m.media-amazon.com/images/I/41S-0X0vlZL._AC_.jpg","https://m.media-amazon.com/images/I/41ZSf5lM-dL._AC_.jpg"]', 1, N'SmartLamp', N'[0]', NULL);
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, Functionalities, LocationType) VALUES(N'Kasa Smart Motion Sensor Switch', N'abcdef', N'Multicolor smart lamp, Works with Alexa', N'["https://m.media-amazon.com/images/I/51IUi36QY2L._SL1300_.jpg","https://m.media-amazon.com/images/I/51Vo5p9j-\u002BL._SL1300_.jpg","https://m.media-amazon.com/images/I/41m8AizPRAL.jpg"]', 1, N'MotionSensor', N'[0]', NULL);
INSERT INTO sqlserver.dbo.Devices (Name, Model, Description, PhotoURLs, CompanyId, Kind, Functionalities, LocationType) VALUES(N'Ring Stick Up Cam Battery ', N'abcdef', N'Weather-Resistant Outdoor Camera, Live View, Color Night Vision, Two-way Talk, Motion alerts, Works with Alexa | White', N'["https://m.media-amazon.com/images/I/419BrDcflML.jpg","https://m.media-amazon.com/images/I/41EMzkDF2KL._SL1000_.jpg","https://m.media-amazon.com/images/I/61ktgGAXj-L._SL1000_.jpg","https://m.media-amazon.com/images/I/61YB1QUE1WL._SL1000_.jpg"]', 1, N'SecurityCamera', N'[0,1]', 1);

-- CREAR HOGARES
INSERT INTO sqlserver.dbo.Homes (OwnerId, Alias, Street, DoorNumber, Latitude, Longitude, MaximumMembers, HomeOwnerId) VALUES(3, N'My lovely home', N'Fifth St', 234, 547.458457, 23.6457, 1, 3);
INSERT INTO sqlserver.dbo.Homes (OwnerId, Alias, Street, DoorNumber, Latitude, Longitude, MaximumMembers, HomeOwnerId) VALUES(3, N'Holidays home', N'Pearl St', 233, 41.7637, 72.6851, 5, 3);

-- CREAR CUARTOS
INSERT INTO sqlserver.dbo.Room (Name, HomeId) VALUES(N'Mi cuarto', 1);

-- CREAR UNIDADES DE DISPOSITIVOS
INSERT INTO sqlserver.dbo.DeviceUnits (HardwareId, Name, DeviceId, IsConnected, RoomId, HomeId, Status) VALUES(N'ACFC9230-BBEB-4355-A058-3939CC198E6B', N'My favourite device', 2, 1, 1, 1, N'Off');
INSERT INTO sqlserver.dbo.DeviceUnits (HardwareId, Name, DeviceId, IsConnected, RoomId, HomeId, Status) VALUES(N'8DAAE6A6-53A2-4D3F-8827-7EA578A2D1F6', N'Ring Stick Up Cam Battery ', 4, 0, NULL, 1, N'');
INSERT INTO sqlserver.dbo.DeviceUnits (HardwareId, Name, DeviceId, IsConnected, RoomId, HomeId, Status) VALUES(N'A9B50BF7-D294-4E06-B64A-8C609F318CCE', N'Window Alarm Sensors 4 Pack', 1, 1, NULL, 1, N'Open');
INSERT INTO sqlserver.dbo.DeviceUnits (HardwareId, Name, DeviceId, IsConnected, RoomId, HomeId, Status) VALUES(N'D296F2B1-8AF9-463D-87EF-E6CBE36EED5E', N'Kasa Smart Motion Sensor Switch', 3, 1, NULL, 1, N'');

-- CREAR MIEMBROS DE HOGAR
INSERT INTO sqlserver.dbo.Members (UserId, HasPermissionToListDevices, HasPermissionToAddADevice, ReceivesNotifications, HomeId) VALUES(4, 1, 0, 1, 1);


-- CREAR NOTIFICACIONES
INSERT INTO sqlserver.dbo.Notifications (HomeId, MemberId, DeviceUnitHardwareId, Event, CreatedAt, [Read], ReadAt) VALUES(1, 1, N'A9B50BF7-D294-4E06-B64A-8C609F318CCE', N'The functionality OpenClosed has been executed in device a9b50bf7-d294-4e06-b64a-8c609f318cce from home 1.', '2024-11-21 01:25:14.923', 1, '2024-11-21 01:35:14.923');



-- CREAR SESIONES
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'19443BF1-9DFF-40D6-045B-08DD09DEA335', 1);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'BF398952-8694-4329-045D-08DD09DEA335', 3);
INSERT INTO sqlserver.dbo.Sessions (Id, UserId) VALUES(N'29E73BB6-9A33-4E1E-045C-08DD09DEA335', 4);