import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { LogInComponent } from './logIn/logIn.component';
import { HomePageComponent } from './homePage/homePage.component';
import { SignUpHomeOwnerComponent } from './signUpHomeOwner/signUpHomeOwner.component';
import { AccountComponent } from './account/account.component';
import { HttpClientModule } from '@angular/common/http';
import {CreateHomeComponent} from './createHome/createHome.component';
import {HomesOfHomeOwnerComponent} from './homesOfHomeOwner/homesOfHomeOwner.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';
import { AdministratorPanelComponent } from './pages/administrator/administrator-panel/administrator-panel.component';
import { CreateUserComponent } from './pages/administrator/create-user/create-user.component';
import { DeleteAdminComponent } from './pages/administrator/delete-admin/delete-admin.component';
import {ImportComponent} from './import/import.component';
import { UserPanelComponent } from './pages/user-panel/user-panel.component';
import { CompanyOwnerPanelComponent } from './pages/company-owner/company-owner-panel/company-owner-panel.component';
import {DevicesListComponent} from './devices-list/devices-list.component';

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'login', component: LogInComponent },
  { path: 'home-owners', component: SignUpHomeOwnerComponent },
  { path: 'account', component: AccountComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'homes', component: CreateHomeComponent },
  { path: 'homes-home-owner', component: HomesOfHomeOwnerComponent },
  { path: 'administrator', component: AdministratorPanelComponent },
  { path: 'administrator/new-admin', component: CreateUserComponent },
  { path: 'administrator/new-companyOwner', component: CreateUserComponent },
  { path: 'administrator/delete-admin', component: DeleteAdminComponent },
  { path: 'imports', component: ImportComponent },
  { path: 'home/user-panel', component: UserPanelComponent },
  { path: 'home/company-owner-panel', component: CompanyOwnerPanelComponent },
  { path: 'home/devices-list', component: DevicesListComponent }

];

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    NgbModule,
    NgxPaginationModule
  ],
  declarations: [
    AppComponent,
    LogInComponent,
    HomePageComponent,
    SignUpHomeOwnerComponent,
    AccountComponent,
    CreateHomeComponent,
    HomesOfHomeOwnerComponent,
    AdministratorPanelComponent,
    CreateUserComponent,
    ImportComponent,
    HomesOfHomeOwnerComponent,
    AdministratorPanelComponent,
    CreateUserComponent,
    HomesOfHomeOwnerComponent,
    ImportComponent,
    DeleteAdminComponent,
    UserPanelComponent,
    CompanyOwnerPanelComponent,
    DevicesListComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
