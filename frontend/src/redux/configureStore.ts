import { configureStore } from '@reduxjs/toolkit';
import api from './api';
import playlistSlice from './playlist.slice';

export const store = configureStore({
  reducer: {
    [api.reducerPath]: api.reducer,
    playlist: playlistSlice
  },
  middleware: (getDefaultMiddeware) =>
    getDefaultMiddeware().concat(api.middleware)
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
