import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { LoginPanelComponent } from './pages/login/panel/login-panel.component';
import { HomePanelComponent } from './pages/home/panel/home-panel.component';
import { CreateHomeOwnerComponent } from './pages/home-owners/create/create-home-owner.component';
import { HomeOwnersPanelComponent } from './pages/home-owners/panel/home-owners-panel.component';
import { HttpClientModule } from '@angular/common/http';
import {CreateHomeComponent} from './pages/home-owners/homes/create/createHome.component';
import {HomeOwnersHomesComponent} from './pages/home-owners/homes/panel/home-owners-homes.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';
import { AdministratorPanelComponent } from './pages/administrators/panel/administrator-panel.component';
import { CreateUserComponent } from './pages/administrators/create-user/create-user.component';
import { DeleteAdminComponent } from './pages/administrators/delete-admin/delete-admin.component';
import { ImportDeviceComponent } from './pages/company-owners/import-device/import-device.component';
import { UserPanelComponent } from './pages/user-panel/user-panel.component';
import { CompanyOwnersPanelComponent } from './pages/company-owners/panel/company-owners-panel.component';
import {DevicesPanelComponent} from './pages/devices/panel/devices-panel.component';
import {NgOptimizedImage} from '@angular/common';
import {ListUsersComponent} from './pages/administrators/list-users/list-users.component';
import {ListCompaniesComponent} from './pages/administrators/list-companies/list-companies.component';
import {CreateDeviceComponent} from './pages/company-owners/create-device/create-device.component';
import {CreateCompanyComponent} from './pages/company-owners/create-company/create-company.component';
import {NotificationsPanelComponent} from './pages/notifications/panel/notifications-panel.component';
import {administratorGuard} from './guard/administrator/administrator.guard';
import {loggedGuard} from './guard/logged/logged.guard';
import {notLoggedGuard} from './guard/not-logged/not-logged.guard';
import {homeOwnerGuard} from './guard/homeoOwner/home-owner.guard';
import {companyOwnerGuardGuard} from './guard/companyOwner/company-owner.guard.guard';

const routes: Routes = [
  { path: '', component: HomePanelComponent, canActivate:[notLoggedGuard]},
  { path: 'login', component: LoginPanelComponent, canActivate:[notLoggedGuard]},
  { path: 'devices', component: DevicesPanelComponent, canActivate:[loggedGuard]},
  { path: 'administrators', component: AdministratorPanelComponent, canActivate:[administratorGuard]},
  { path: 'administrators/create-user', component: CreateUserComponent, canActivate:[administratorGuard] },
  { path: 'administrators/delete-admin', component: DeleteAdminComponent, canActivate:[administratorGuard] },
  { path: 'administrators/list-users', component: ListUsersComponent, canActivate:[administratorGuard] },
  { path: 'administrators/list-companies', component: ListCompaniesComponent, canActivate:[administratorGuard] },
  { path: 'home-owners', component: HomeOwnersPanelComponent, canActivate:[homeOwnerGuard] },
  { path: 'home-owners/create', component: CreateHomeOwnerComponent },
  { path: 'home-owners/homes', component: HomeOwnersHomesComponent,canActivate:[homeOwnerGuard] },
  { path: 'home-owners/homes/create', component: CreateHomeComponent, canActivate:[homeOwnerGuard]},
  { path: 'company-owners', component: CompanyOwnersPanelComponent, canActivate: [companyOwnerGuardGuard] },
  { path: 'company-owners/import-device', component: ImportDeviceComponent, canActivate: [companyOwnerGuardGuard]  },
  { path: 'company-owners/create-device', component: CreateDeviceComponent,canActivate: [companyOwnerGuardGuard]  },
  { path: 'company-owners/create-company', component: CreateCompanyComponent , canActivate: [companyOwnerGuardGuard] },
  { path: 'home', component: HomePanelComponent , canActivate:[notLoggedGuard]},
  { path: 'home/user-panel', component: UserPanelComponent },
  { path: 'notifications', component: NotificationsPanelComponent , canActivate:[loggedGuard]},
];

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    NgbModule,
    NgxPaginationModule,
    NgOptimizedImage
  ],
  declarations: [
    AppComponent,
    LoginPanelComponent,
    HomePanelComponent,
    CreateHomeOwnerComponent,
    HomeOwnersPanelComponent,
    CreateHomeComponent,
    HomeOwnersHomesComponent,
    AdministratorPanelComponent,
    CreateUserComponent,
    ImportDeviceComponent,
    HomeOwnersHomesComponent,
    AdministratorPanelComponent,
    CreateUserComponent,
    HomeOwnersHomesComponent,
    DeleteAdminComponent,
    UserPanelComponent,
    CompanyOwnersPanelComponent,
    DevicesPanelComponent,
    ListUsersComponent,
    ListCompaniesComponent,
    CreateDeviceComponent,
    CreateCompanyComponent,
    NotificationsPanelComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
