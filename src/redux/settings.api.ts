import { Directory, MediaType } from '../models/Directory';
import api from './api';

const settingsApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getDirectories: builder.query<Directory[], void>({
      query: () => 'settings/directories',
      providesTags: (result) => {
        return ['Directories'];
      }
    }),
    loadVideos: builder.mutation<void, void>({
      query: () => ({
        url: 'settings/updateVideos',
        method: 'POST'
      }),
      invalidatesTags: ['Videos']
    }),
    cleanVideos: builder.mutation<void, void>({
      query: () => ({
        url: 'settings/cleanVideos',
        method: 'DELETE'
      }),
      invalidatesTags: ['Videos']
    }),
    addDirectory: builder.mutation<
      void,
      {
        directory: {
          directoryPath: string;
          isIncludeSubdirectories: boolean;
          mediaType: MediaType;
        },
        loadContent: boolean
      }
    >({
      query: (params) => ({
        url: 'settings/addDirectory',
        method: 'POST',
        params: { loadContent: params.loadContent },
        body: params.directory
      }),
      invalidatesTags: ['Videos', 'Directories']
    })
  })
});

export const {
  useGetDirectoriesQuery,
  useLoadVideosMutation,
  useCleanVideosMutation,
  useAddDirectoryMutation
} = settingsApi;
