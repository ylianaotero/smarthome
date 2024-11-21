import {CanActivateFn, Router} from '@angular/router';
import {userRetrieveModel} from '../../pages/home-owners/create/signUpUserModel';

export const loggedGuard: CanActivateFn = (route, state) => {
  const storedUser = localStorage.getItem('user');
  let res: userRetrieveModel | null = null;
  const router = new Router();
  if(storedUser){
    res = JSON.parse(storedUser) as userRetrieveModel;
    if (res) {
      return true;
    }
  }
  router.navigate(['home']);
  return false;
};
