import { alpha, Box, Paper, Tab, Tabs } from '@mui/material';
import { useState } from 'react';
import { useSearchParams } from 'react-router-dom';

import DirectoriesSettings from './DirectoriesSettings';
import VideosDataGrid from './VideosDataGrid';

interface PropType {}

const SettingsPage = (props: PropType) => {
  const [searchParams, setSearchParams] = useSearchParams();
  const [activeTab, setActiveTab] = useState(Number.parseInt(searchParams.get('tab') || '0'));

  const handleTabChange = (_e: React.SyntheticEvent<Element, Event>, v: number) => {
    setActiveTab(v);
    setSearchParams({ tab: v?.toString() });
  };

  return (
    <>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          flexGrow: 1,
          overflow: 'hidden',
          p: 1
        }}>
        <Box sx={{ m: 1 }}>
          <Tabs onChange={handleTabChange} value={activeTab}>
            <Tab label="Directories"></Tab>
            <Tab label="Videos"></Tab>
          </Tabs>
        </Box>
        <Paper
          sx={{
            flexGrow: 1,
            m: 1,
            backgroundColor: (theme) => alpha(theme.palette.background.paper, 0.7),
            position: 'relative',
            overflow: 'auto'
          }}>
          {activeTab === 0 && <DirectoriesSettings />}
          {activeTab === 1 && <VideosDataGrid />}
        </Paper>
      </Box>
    </>
  );
};

export default SettingsPage;
