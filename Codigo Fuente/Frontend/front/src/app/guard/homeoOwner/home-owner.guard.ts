import { CanActivateFn } from '@angular/router';
import { userRetrieveModel } from '../../pages/home-owners/create/signUpUserModel';
import { Router } from '@angular/router';

export const homeOwnerGuard: CanActivateFn = (route, state) => {
  const storedUser = localStorage.getItem('user');
  let res: userRetrieveModel | null = null;
  const router = new Router();

  if (storedUser) {
    res = JSON.parse(storedUser) as userRetrieveModel;
    if (res === undefined || res === null) {
      router.navigate(['home']);
      return false;
    }

    if (res.roles && res.roles.some(role => role.kind === 'HomeOwner')) {
      return true;
    } else if (res.roles && res.roles.some(role => role.kind === 'Administrator' && res?.roles.length === 1)) {
      router.navigate(['administrators']);
      return false;
    } else if (res.roles && res.roles.some(role => role.kind === 'CompanyOwner' && res?.roles.length === 1)) {
      router.navigate(['company-owners']);
      return false;
    } else if (res?.roles.length > 1 && !res.roles.some(role => role.kind === 'HomeOwner')) {
      router.navigate(['home/user-panel']);
      return false;
    }
  }
  router.navigate(['home']);
  return false;
};

