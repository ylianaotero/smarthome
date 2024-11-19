import { CanActivateFn } from '@angular/router';

export const administratorGuard: CanActivateFn = (route, state) => {  
  const storedUser = localStorage.getItem('user');
  if (storedUser) {
    const user = JSON.parse(storedUser);
    if (user.roles && user.roles.includes('administrator')) {
      return true;
    }
  }
  return false;
};
