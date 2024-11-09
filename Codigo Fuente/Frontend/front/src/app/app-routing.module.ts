import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { LogInComponent } from './logIn/logIn.component';
import { HomePageComponent } from './homePage/homePage.component';
import { SignUpHomeOwnerComponent } from './signUpHomeOwner/signUpHomeOwner.component';
import {AccountComponent} from './account/account.component';
import {CreateHomeComponent} from './createHome/createHome.component';
import {HomesOfHomeOwnerComponent} from './homesOfHomeOwner/homesOfHomeOwner.component';
import {ImportComponent} from './import/import.component';


export const routes: Routes = [
  { path: '', component: AppComponent },
  { path: 'login', component: LogInComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'home-owners', component: SignUpHomeOwnerComponent },
  { path: 'account', component: AccountComponent },
  { path: 'homes', component: CreateHomeComponent },
  { path: 'homes-home-owner', component: HomesOfHomeOwnerComponent },
  { path: 'imports', component: ImportComponent }
];
