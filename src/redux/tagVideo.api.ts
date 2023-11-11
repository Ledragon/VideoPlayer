import { TagVideo } from '../models/TagVideo';
import api from './api';

const tagVideoApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getTagVideos: builder.query<TagVideo[], void>({
      query: () => 'tagVideo',
      providesTags: (result) => {
        return ['TagVideos'];
      }
    }),
    addTagVideos: builder.mutation<void, TagVideo[]>({
      query: (data) => ({ url: 'tagVideo/add', method: 'POST', body: data }),
      invalidatesTags: ['TagVideos']
    })
  })
});

export const { useGetTagVideosQuery, useAddTagVideosMutation } = tagVideoApi;
