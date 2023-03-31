import { Directory } from '../models/Directory';
import api from './api';

const settingsApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getDirectories: builder.query<Directory[], void>({
      query: () => 'settings/directories',
      providesTags: (result) => {
        return ['Directories'];
      }
    })
  })
});

export const { useGetDirectoriesQuery } = settingsApi;
