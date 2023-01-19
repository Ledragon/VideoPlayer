import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { apiUrl } from '../fetchHelper';
import { Video } from '../models/Video';
import { VideoMetadata } from '../models/VideoMetadata';

const api = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({ baseUrl: apiUrl }),
  tagTypes: ['Videos'],
  endpoints: (builder) => ({
    getVideos: builder.query<Video[], void>({
      query: () => 'videos',
      providesTags: (result) => {
        return ['Videos'];
      }
    }),
    updateVideo: builder.mutation<Video, Video>({
      query: (video) => ({
        url: 'videos',
        method: 'PUT',
        body: video
      }),
      invalidatesTags: ['Videos']
    }),
    getMetadata: builder.query<VideoMetadata[], void>({
      query: () => 'metadata'
    }),
    getMetadataForVideo: builder.query<VideoMetadata, string>({
      query: (filePath: string) =>
        'metadata?' + new URLSearchParams({ filePath })
    }),
    getThumbnails: builder.mutation<string[], { filePath: string; count: number }>(
      {
        query: (data) => ({
          url: 'thumbnails',
          params: data,
          method: 'GET'
        })
      }
    )
  })
});

export default api;

export const { useGetVideosQuery, useUpdateVideoMutation, useGetThumbnailsMutation } = api;
