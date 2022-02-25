import { TestBed } from '@angular/core/testing';

import { HaberciService } from './haberci.service';

describe('HaberciService', () => {
  let service: HaberciService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HaberciService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
