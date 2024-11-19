import { CanActivateFn } from '@angular/router';

export const homeOwnerGuard: CanActivateFn = (route, state) => {  
  const storedUser = localStorage.getItem('user');
  if (storedUser) {
    const user = JSON.parse(storedUser);
    if (user.roles && user.roles.includes('homeowner')) {
      return true;
    }
  }
  return false;
};
