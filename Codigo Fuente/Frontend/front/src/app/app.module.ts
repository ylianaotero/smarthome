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

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'login', component: LogInComponent },
  { path: 'home-owners', component: SignUpHomeOwnerComponent },
  { path: 'account', component: AccountComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'homes', component: CreateHomeComponent },
  { path: 'homes-home-owner', component: HomesOfHomeOwnerComponent }
];

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(routes),
    HttpClientModule
  ],
  declarations: [
    AppComponent,
    LogInComponent,
    HomePageComponent,
    SignUpHomeOwnerComponent,
    AccountComponent,
    CreateHomeComponent,
    HomesOfHomeOwnerComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
