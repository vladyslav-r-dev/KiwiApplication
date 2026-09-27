import { Router } from '@angular/router';
import { vi } from 'vitest';
import { of } from 'rxjs';
import { signal } from '@angular/core';
import { Login } from './login';
import { AuthService } from '../../services/auth.service';

describe('Google login', () => {
  it('renders outside injection context and retains the credential callback', () => {
    const initialize = vi.fn();
    const renderButton = vi.fn();
    vi.stubGlobal('google', { accounts: { id: { initialize, renderButton } } });
    const button = document.createElement('div');
    button.id = 'googleButton';
    document.body.appendChild(button);
    try {
      const googleLogin = vi.fn().mockReturnValue(of({ accessToken: 'access' }));
      const accessToken = signal<string | null>(null);
      const navigate = vi.fn();
      const component = new Login({ googleLogin, accessToken } as unknown as AuthService, { navigate } as unknown as Router);
      expect(() => component.ngAfterViewInit()).not.toThrow();
      expect(renderButton).toHaveBeenCalledWith(button, expect.any(Object));
      initialize.mock.calls[0][0].callback({ credential: 'credential' });
      expect(googleLogin).toHaveBeenCalledWith('credential');
      expect(accessToken()).toBe('access');
      expect(navigate).toHaveBeenCalledWith(['/flights']);
    } finally {
      button.remove();
      vi.unstubAllGlobals();
    }
  });
});
