import { TestBed } from '@angular/core/testing';

import { AppToastService } from './app-toast.service';

describe('AppToastService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AppToastService = TestBed.get(AppToastService);
    expect(service).toBeTruthy();
  });
});
