import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignupViewComponent } from './signup-view/signup-view.component';
import { SigninViewComponent } from './signin-view/signin-view.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { UpdateProductViewComponent } from './update-product-view/update-product-view.component';
import { UpdateUserByAdminViewComponent } from './update-user-by-admin-view/update-user-by-admin-view.component';
import { CreateUserByAdminComponent } from './create-user-by-admin/create-user-by-admin.component';
import { UsersAdminViewComponent } from './users-admin-view/users-admin-view.component';
import { ProductAdminViewComponent } from './product-admin-view/product-admin-view.component';
import { CreateProductAdminViewComponent } from './create-product-admin-view/create-product-admin-view.component';
import { PurchaseViewComponent } from './purchase-view/purchase-view.component';
import { UpdataSelfDataViewComponent } from './updata-self-data-view/updata-self-data-view.component';
import { PurchaseHistoryComponent } from './purchase-history/purchase-history.component';
import {PurchaseHistoryAdminComponent} from './purchase-history-admin/purchase-history-admin.component';

const routes: Routes = [
  {path:'', component:LandingPageComponent},
  {path:'signup', component:SignupViewComponent},
  {path:'signin', component:SigninViewComponent},
  {path:'admin/updateProduct', component:UpdateProductViewComponent},
  {path:'admin/updateUser',component:UpdateUserByAdminViewComponent},
  {path:'admin/createUser',component:CreateUserByAdminComponent},
  {path:'admin/users',component:UsersAdminViewComponent},
  {path:'admin/products',component: ProductAdminViewComponent},
  {path:'admin/createProduct',component: CreateProductAdminViewComponent},
  {path: 'purchases', component: PurchaseViewComponent},
  {path:'profile',component: UpdataSelfDataViewComponent},
  {path: 'purchases/history',component: PurchaseHistoryComponent},
  {path: 'admin/history',component: PurchaseHistoryAdminComponent},
  {path: '**', redirectTo: ''}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
