import {
  EntityState,
  createEntityAdapter,
  createSelector
} from '@reduxjs/toolkit';
import { Tag } from '../models/Tag';
import api from './api';
import { RootState } from './configureStore';

const entityAdapter = createEntityAdapter<Tag>({
  selectId: (t) => t.id,
  sortComparer: (a, b) => a.value.localeCompare(b.value)
});

const initialState = entityAdapter.getInitialState();

const tagApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getTags: builder.query<EntityState<Tag>, void>({
      query: () => 'tags',
      providesTags: (result) => {
        return ['Tags'];
      },
      transformResponse: (response: Tag[]) =>
        entityAdapter.setAll(initialState, response)
    }),
    addTag: builder.mutation<EntityState<Tag>, Tag>({
      query: (tag) => ({
        url: 'tags',
        method: 'POST',
        body: tag
      }),
      invalidatesTags: (result) => {
        return ['Tags'];
      },
      transformResponse: (response: Tag) =>
        entityAdapter.setOne(initialState, response)
    })
  })
});

const selector = tagApi.endpoints.getTags.select();
const tagsSelector = createSelector(selector, (tags) => tags.data);
export const {
  selectAll: getTagList,
  selectById: getTag,
  selectEntities: getTagDictionary
} = entityAdapter.getSelectors<RootState>(
  (state) => tagsSelector(state) ?? initialState
);

export const { useGetTagsQuery, useAddTagMutation } = tagApi;
